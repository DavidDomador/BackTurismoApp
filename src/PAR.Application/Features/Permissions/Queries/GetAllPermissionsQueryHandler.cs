using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Permissions.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Permissions.Queries;

public class GetAllPermissionsQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetAllPermissionsQuery, Result<IEnumerable<PermissionDto>>>
{
    public async Task<Result<IEnumerable<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await roleRepository.GetAllPermissionsAsync(cancellationToken);
        var dtos = permissions.Select(p => new PermissionDto
        {
            Id          = p.Id,
            Name        = p.Name,
            Module      = p.Module,
            Description = p.Description
        });
        return Result<IEnumerable<PermissionDto>>.Success(dtos);
    }
}
