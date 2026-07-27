using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Users.Commands;

public record ChangePasswordCommand(int UserId, string CurrentPassword, string NewPassword) : IRequest<Result>;
