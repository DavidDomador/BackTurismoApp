using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Queries;

public record GetUserRolesQuery(int UserId) : IRequest<Result<IEnumerable<int>>>;
