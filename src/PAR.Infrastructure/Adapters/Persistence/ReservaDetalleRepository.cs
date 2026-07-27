using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class ReservaDetalleRepository(AppDbContext context) : IReservaDetalleRepository
{
    public async Task<IEnumerable<ReservaDetalle>> GetByReservaAsync(int iCodReserva, CancellationToken ct = default) =>
        await context.ReservaDetalles
            .Include(d => d.Cliente)
            .Include(d => d.EstadoClienteDetalle)
            .Where(d => d.ICodReserva == iCodReserva && d.IsActive)
            .OrderBy(d => d.ICodReservaDetalle)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<ReservaDetalle?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.ReservaDetalles
            .Include(d => d.Cliente)
            .Include(d => d.EstadoClienteDetalle)
            .FirstOrDefaultAsync(d => d.ICodReservaDetalle == id && d.IsActive, ct);

    public async Task<ReservaDetalle> CreateAsync(ReservaDetalle detalle, CancellationToken ct = default)
    {
        context.ReservaDetalles.Add(detalle);
        await context.SaveChangesAsync(ct);
        await context.Entry(detalle).Reference(d => d.Cliente).LoadAsync(ct);
        await context.Entry(detalle).Reference(d => d.EstadoClienteDetalle).LoadAsync(ct);
        return detalle;
    }

    public async Task<ReservaDetalle> UpdateAsync(ReservaDetalle detalle, CancellationToken ct = default)
    {
        context.ReservaDetalles.Update(detalle);
        await context.SaveChangesAsync(ct);
        await context.Entry(detalle).Reference(d => d.Cliente).LoadAsync(ct);
        await context.Entry(detalle).Reference(d => d.EstadoClienteDetalle).LoadAsync(ct);
        return detalle;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.ReservaDetalles
            .Where(d => d.ICodReservaDetalle == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.IsActive, false)
                .SetProperty(d => d.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.ReservaDetalles.AnyAsync(d => d.ICodReservaDetalle == id && d.IsActive, ct);
}
