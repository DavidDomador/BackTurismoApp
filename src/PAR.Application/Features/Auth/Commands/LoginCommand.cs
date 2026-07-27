using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Auth.DTOs;

namespace PAR.Application.Features.Auth.Commands;

public record LoginCommand(string Username, string Password, string IpAddress) : IRequest<Result<LoginResponseDto>>;
