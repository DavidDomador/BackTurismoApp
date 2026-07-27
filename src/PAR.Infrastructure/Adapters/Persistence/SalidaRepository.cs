using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class SalidaRepository(AppDbContext context) : ISalidaRepository
{
    private IQueryable<Salida> WithIncludes() =>
        context.Salidas
            .Include(s => s.SalidaReservas)
                .ThenInclude(sr => sr.Reserva)
                    .ThenInclude(r => r!.Paquete)
            .Include(s => s.Guia)
            .Include(s => s.ChoferVehiculo).ThenInclude(cv => cv!.Chofer)
            .Include(s => s.ChoferVehiculo).ThenInclude(cv => cv!.Vehiculo);

    public async Task<IEnumerable<Salida>> GetAllAsync(CancellationToken ct = default) =>
        await WithIncludes()
            .Where(s => s.IsActive)
            .OrderByDescending(s => s.FechaSalida)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Salida?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await WithIncludes()
            .Where(s => s.ICodSalida == id && s.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

    public async Task<Salida> CreateAsync(Salida salida, CancellationToken ct = default)
    {
        salida.CreatedAt = DateTime.UtcNow;
        context.Salidas.Add(salida);
        await context.SaveChangesAsync(ct);
        return salida;
    }

    public async Task<Salida> UpdateAsync(Salida salida, IList<SalidaReserva> newReservas, CancellationToken ct = default)
    {
        // Reemplazar reservas: borrar las existentes e insertar las nuevas
        var existing = await context.SalidaReservas
            .Where(sr => sr.ICodSalida == salida.ICodSalida)
            .ToListAsync(ct);
        context.SalidaReservas.RemoveRange(existing);

        context.Salidas.Update(salida);
        await context.SalidaReservas.AddRangeAsync(newReservas, ct);
        await context.SaveChangesAsync(ct);
        return salida;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var salida = await context.Salidas.FindAsync([id], ct);
        if (salida is null) return;
        salida.IsActive  = false;
        salida.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Salidas.AnyAsync(s => s.ICodSalida == id && s.IsActive, ct);
}
