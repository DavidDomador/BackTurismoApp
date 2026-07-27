using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class ItinerarioRepository(AppDbContext context) : IItinerarioRepository
{
    public async Task<IEnumerable<Itinerario>> GetByPaqueteAsync(int iCodPaquete, CancellationToken ct = default) =>
        await context.Itinerarios
            .Where(i => i.ICodPaquete == iCodPaquete && i.IsActive)
            .OrderBy(i => i.INombre)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Itinerario?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Itinerarios
            .FirstOrDefaultAsync(i => i.ICodItinerario == id && i.IsActive, ct);

    public async Task<Itinerario> CreateAsync(Itinerario itinerario, CancellationToken ct = default)
    {
        context.Itinerarios.Add(itinerario);
        await context.SaveChangesAsync(ct);
        return itinerario;
    }

    public async Task<Itinerario> UpdateAsync(Itinerario itinerario, CancellationToken ct = default)
    {
        context.Itinerarios.Update(itinerario);
        await context.SaveChangesAsync(ct);
        return itinerario;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Itinerarios
            .Where(i => i.ICodItinerario == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(i => i.IsActive, false)
                .SetProperty(i => i.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Itinerarios.AnyAsync(i => i.ICodItinerario == id && i.IsActive, ct);
}
