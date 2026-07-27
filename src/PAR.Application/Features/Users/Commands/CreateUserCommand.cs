using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;

namespace PAR.Application.Features.Users.Commands;

public record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string? FirstName,
    string? LastName) : IRequest<Result<UserDto>>;
