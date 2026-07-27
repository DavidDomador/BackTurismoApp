using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Menus.DTOs;

namespace PAR.Application.Features.Menus.Queries;

public record GetMenusQuery(IEnumerable<string> UserPermissions) : IRequest<Result<IEnumerable<MenuDto>>>;
