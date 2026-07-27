using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;

namespace PAR.Application.Features.Users.Queries;

public record GetUserPermissionSummaryQuery(int UserId) : IRequest<Result<UserPermissionSummaryDto>>;
