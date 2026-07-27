using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Menus.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Menus.Queries;

public class GetMenusQueryHandler(IMenuRepository menuRepository)
    : IRequestHandler<GetMenusQuery, Result<IEnumerable<MenuDto>>>
{
    public async Task<Result<IEnumerable<MenuDto>>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        var menus = await menuRepository.GetAllActiveAsync(cancellationToken);

        var permissions = request.UserPermissions.ToHashSet();

        var dtos = menus.OrderBy(m => m.Order)
            .Select(m => new MenuDto
            {
                Id    = m.Id,
                Name  = m.Name,
                Icon  = m.Icon,
                Order = m.Order,
                Items = m.Items
                    .Where(i => i.IsActive &&
                                (string.IsNullOrEmpty(i.RequiredPermission) ||
                                 permissions.Contains(i.RequiredPermission)))
                    .OrderBy(i => i.Order)
                    .Select(i => new MenuItemDto
                    {
                        Id                 = i.Id,
                        Name               = i.Name,
                        Route              = i.Route,
                        Icon               = i.Icon,
                        RequiredPermission = i.RequiredPermission,
                        Order              = i.Order
                    }).ToList()
            })
            .Where(m => m.Items.Count > 0);

        return Result<IEnumerable<MenuDto>>.Success(dtos);
    }
}
