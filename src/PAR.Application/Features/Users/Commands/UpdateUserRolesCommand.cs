using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Commands;

public record UpdateUserRolesCommand(int UserId, IList<int> RoleIds) : IRequest<Result>;
