using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class GuiaRepository(AppDbContext context) : IGuiaRepository
{
    public async Task<IEnumerable<Guia>> GetAllAsync(CancellationToken ct = default) =>
        await context.Guias.Where(g => g.IsActive).OrderBy(g => g.GApellidos).ThenBy(g => g.GNombre).AsNoTracking().ToListAsync(ct);

    public async Task<Guia?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Guias.FirstOrDefaultAsync(g => g.ICodigoGuia == id, ct);

    public async Task<Guia> CreateAsync(Guia guia, CancellationToken ct = default)
    {
        context.Guias.Add(guia);
        await context.SaveChangesAsync(ct);
        return guia;
    }

    public async Task<Guia> UpdateAsync(Guia guia, CancellationToken ct = default)
    {
        context.Guias.Update(guia);
        await context.SaveChangesAsync(ct);
        return guia;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await context.Guias.Where(g => g.ICodigoGuia == id)
            .ExecuteUpdateAsync(s => s.SetProperty(g => g.IsActive, false).SetProperty(g => g.UpdatedAt, DateTime.UtcNow), ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Guias.AnyAsync(g => g.ICodigoGuia == id && g.IsActive, ct);
}
