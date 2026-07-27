using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class UpdateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
            return Result<UserDto>.Failure("User not found.", 404);

        // Si el email cambió, verificar que no esté en uso
        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existing is not null)
                return Result<UserDto>.Failure("Email already registered.", 409);
        }

        user.UpdateDetails(request.Email, request.FirstName, request.LastName, request.IsActive);
        await userRepository.UpdateAsync(user, cancellationToken);

        return Result<UserDto>.Success(MapToDto(user));
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id        = user.Id,
        Username  = user.Username,
        Email     = user.Email,
        FirstName = user.FirstName,
        LastName  = user.LastName,
        IsActive  = user.IsActive,
        IsLocked  = user.IsLocked,
        CreatedAt = user.CreatedAt
    };
}
