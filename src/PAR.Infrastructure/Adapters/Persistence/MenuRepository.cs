using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class MenuRepository(AppDbContext context) : IMenuRepository
{
    public async Task<IEnumerable<Menu>> GetAllActiveAsync(CancellationToken ct = default) =>
        await context.Menus
            .Where(m => m.IsActive)
            .Include(m => m.Items.Where(i => i.IsActive))
            .OrderBy(m => m.Order)
            .AsNoTracking()
            .ToListAsync(ct);
}
