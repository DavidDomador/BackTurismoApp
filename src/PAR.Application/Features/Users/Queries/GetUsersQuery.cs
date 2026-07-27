using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Users.DTOs;

namespace PAR.Application.Features.Users.Queries;

public record GetUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;
public record GetUserByIdQuery(int Id) : IRequest<Result<UserDto>>;
