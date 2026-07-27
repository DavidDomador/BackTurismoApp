using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<Result>;
