using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Roles.DTOs;

namespace PAR.Application.Features.Roles.Queries;

public record GetRolesQuery() : IRequest<Result<IEnumerable<RoleDto>>>;
