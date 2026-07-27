using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Permissions.DTOs;

namespace PAR.Application.Features.Permissions.Queries;

public record GetAllPermissionsQuery : IRequest<Result<IEnumerable<PermissionDto>>>;
