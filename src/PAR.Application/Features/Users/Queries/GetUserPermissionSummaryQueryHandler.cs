using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Queries;

public class GetUserPermissionSummaryQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IRequestHandler<GetUserPermissionSummaryQuery, Result<UserPermissionSummaryDto>>
{
    public async Task<Result<UserPermissionSummaryDto>> Handle(GetUserPermissionSummaryQuery request, CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsAsync(request.UserId, cancellationToken))
            return Result<UserPermissionSummaryDto>.Failure("User not found.", 404);

        var rolePermIds   = await roleRepository.GetUserRolePermissionIdsAsync(request.UserId, cancellationToken);
        var directPermIds = await userRepository.GetUserDirectPermissionIdsAsync(request.UserId, cancellationToken);

        return Result<UserPermissionSummaryDto>.Success(new UserPermissionSummaryDto
        {
            RolePermissionIds   = rolePermIds,
            DirectPermissionIds = directPermIds
        });
    }
}
