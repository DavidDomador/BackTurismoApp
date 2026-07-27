using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class UpdateUserDirectPermissionsCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserDirectPermissionsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateUserDirectPermissionsCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsAsync(request.UserId, cancellationToken))
            return Result<bool>.Failure("User not found.", 404);

        await userRepository.UpdateUserDirectPermissionsAsync(request.UserId, request.PermissionIds, cancellationToken);
        return Result<bool>.Success(true);
    }
}
