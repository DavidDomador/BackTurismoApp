using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;

namespace PAR.Application.Features.Users.Commands;

public record UpdateUserCommand(
    int    Id,
    string Email,
    string? FirstName,
    string? LastName,
    bool   IsActive) : IRequest<Result<UserDto>>;
