using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class ReservaRepository(AppDbContext context) : IReservaRepository
{
    public async Task<IEnumerable<Reserva>> GetAllAsync(CancellationToken ct = default) =>
        await context.Reservas
            .Include(r => r.Paquete)
            .Include(r => r.ReservaDetalles)
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.RNumeroReserva)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Reserva?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Reservas
            .Include(r => r.Paquete)
            .Include(r => r.ReservaDetalles)
            .FirstOrDefaultAsync(r => r.ICodReserva == id && r.IsActive, ct);

    public async Task<Reserva> CreateAsync(Reserva reserva, CancellationToken ct = default)
    {
        context.Reservas.Add(reserva);
        await context.SaveChangesAsync(ct);
        await context.Entry(reserva).Reference(r => r.Paquete).LoadAsync(ct);
        return reserva;
    }

    public async Task<Reserva> UpdateAsync(Reserva reserva, CancellationToken ct = default)
    {
        context.Reservas.Update(reserva);
        await context.SaveChangesAsync(ct);
        await context.Entry(reserva).Reference(r => r.Paquete).LoadAsync(ct);
        return reserva;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Reservas
            .Where(r => r.ICodReserva == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.IsActive, false)
                .SetProperty(r => r.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Reservas.AnyAsync(r => r.ICodReserva == id && r.IsActive, ct);

    public async Task<int> GetNextNumeroReservaAsync(CancellationToken ct = default)
    {
        var max = await context.Reservas.MaxAsync(r => (int?)r.RNumeroReserva, ct);
        return Math.Max(max ?? 0, 99) + 1;
    }
}
