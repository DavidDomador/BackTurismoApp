using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class UpdateUserRolesCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    : IRequestHandler<UpdateUserRolesCommand, Result>
{
    public async Task<Result> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var exists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!exists)
            return Result.Failure("User not found.", 404);

        await roleRepository.UpdateUserRolesAsync(request.UserId, request.RoleIds, cancellationToken);
        return Result.Success();
    }
}
