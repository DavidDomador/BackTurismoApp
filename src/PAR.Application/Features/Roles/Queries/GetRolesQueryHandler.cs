using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Roles.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Roles.Queries;

public class GetRolesQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
{
    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(cancellationToken);
        var dtos  = roles.Select(r => new RoleDto
        {
            Id          = r.Id,
            Name        = r.Name,
            Description = r.Description,
            IsActive    = r.IsActive
        });
        return Result<IEnumerable<RoleDto>>.Success(dtos);
    }
}
