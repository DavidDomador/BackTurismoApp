using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Queries;

public class GetUserRolesQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    : IRequestHandler<GetUserRolesQuery, Result<IEnumerable<int>>>
{
    public async Task<Result<IEnumerable<int>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var exists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!exists)
            return Result<IEnumerable<int>>.Failure("User not found.", 404);

        var roleIds = await roleRepository.GetUserRoleIdsAsync(request.UserId, cancellationToken);
        return Result<IEnumerable<int>>.Success(roleIds);
    }
}
