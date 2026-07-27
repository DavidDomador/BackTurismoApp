using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Commands;

public record UpdateUserDirectPermissionsCommand(int UserId, IList<int> PermissionIds)
    : IRequest<Result<bool>>;
