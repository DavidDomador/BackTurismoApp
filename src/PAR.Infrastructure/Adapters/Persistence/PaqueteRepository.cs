using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class PaqueteRepository(AppDbContext context) : IPaqueteRepository
{
    public async Task<IEnumerable<Paquete>> GetAllAsync(CancellationToken ct = default) =>
        await context.Paquetes
            .Where(p => p.IsActive)
            .OrderBy(p => p.PNombre)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Paquete?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Paquetes
            .FirstOrDefaultAsync(p => p.ICodPaquete == id && p.IsActive, ct);

    public async Task<Paquete> CreateAsync(Paquete paquete, CancellationToken ct = default)
    {
        context.Paquetes.Add(paquete);
        await context.SaveChangesAsync(ct);
        return paquete;
    }

    public async Task<Paquete> UpdateAsync(Paquete paquete, CancellationToken ct = default)
    {
        context.Paquetes.Update(paquete);
        await context.SaveChangesAsync(ct);
        return paquete;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Paquetes
            .Where(p => p.ICodPaquete == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.IsActive, false)
                .SetProperty(p => p.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Paquetes.AnyAsync(p => p.ICodPaquete == id && p.IsActive, ct);
}
