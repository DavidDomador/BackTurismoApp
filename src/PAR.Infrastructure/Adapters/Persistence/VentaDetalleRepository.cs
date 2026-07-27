using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class VentaDetalleRepository(AppDbContext context) : IVentaDetalleRepository
{
    public async Task<IEnumerable<VentaDetalle>> GetByVentaAsync(int iCodVenta, CancellationToken ct = default) =>
        await context.VentaDetalles
            .Include(d => d.Cliente)
            .Where(d => d.ICodVenta == iCodVenta && d.IsActive)
            .OrderBy(d => d.ICodVentaDetalle)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task CreateManyAsync(IEnumerable<VentaDetalle> detalles, CancellationToken ct = default)
    {
        await context.VentaDetalles.AddRangeAsync(detalles, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteByVentaAsync(int iCodVenta, CancellationToken ct = default) =>
        await context.VentaDetalles
            .Where(d => d.ICodVenta == iCodVenta)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.IsActive, false), ct);
}
