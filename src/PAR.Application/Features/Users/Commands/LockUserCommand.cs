using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Commands;

public record LockUserCommand(int UserId) : IRequest<Result>;
public record UnlockUserCommand(int UserId) : IRequest<Result>;
