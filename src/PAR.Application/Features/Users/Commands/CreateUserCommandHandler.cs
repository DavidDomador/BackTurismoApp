using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;
using PAR.Application.Ports;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService) : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (existing != null)
            return Result<UserDto>.Failure("Username already exists.", 409);

        var existingEmail = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingEmail != null)
            return Result<UserDto>.Failure("Email already registered.", 409);

        var passwordHash = passwordService.HashPassword(request.Password);

        var user = User.Create(
            request.Username,
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName);

        var created = await userRepository.CreateAsync(user, cancellationToken);

        // Save initial password history
        await userRepository.AddPasswordHistoryAsync(new UserPasswordHistory
        {
            UserId = created.Id,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return Result<UserDto>.Success(MapToDto(created), 201);
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        IsActive = user.IsActive,
        IsLocked = user.IsLocked,
        CreatedAt = user.CreatedAt
    };
}
