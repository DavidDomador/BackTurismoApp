using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class DefinicionRepository(AppDbContext context) : IDefinicionRepository
{
    // ── Definicion ────────────────────────────────────────────────────

    public async Task<IEnumerable<Definicion>> GetAllAsync(CancellationToken ct = default) =>
        await context.Definiciones
            .Include(d => d.Detalles.Where(dd => dd.IsActive))
            .Where(d => d.IsActive)
            .OrderBy(d => d.DNombre)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Definicion?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Definiciones
            .Include(d => d.Detalles.Where(dd => dd.IsActive))
            .FirstOrDefaultAsync(d => d.IdDef == id && d.IsActive, ct);

    public async Task<Definicion> CreateAsync(Definicion definicion, CancellationToken ct = default)
    {
        context.Definiciones.Add(definicion);
        await context.SaveChangesAsync(ct);
        return definicion;
    }

    public async Task<Definicion> UpdateAsync(Definicion definicion, CancellationToken ct = default)
    {
        context.Definiciones.Update(definicion);
        await context.SaveChangesAsync(ct);
        return definicion;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Definiciones
            .Where(d => d.IdDef == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.IsActive, false)
                .SetProperty(d => d.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Definiciones.AnyAsync(d => d.IdDef == id && d.IsActive, ct);

    // ── DefinicionDetalle ──────────────────────────────────────────────

    public async Task<IEnumerable<DefinicionDetalle>> GetDetallesByDefIdAsync(int idDef, CancellationToken ct = default) =>
        await context.DefinicionDetalles
            .Where(dd => dd.IdDef == idDef && dd.IsActive)
            .OrderBy(dd => dd.DdValue)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<DefinicionDetalle?> GetDetalleByIdAsync(int id, CancellationToken ct = default) =>
        await context.DefinicionDetalles.FirstOrDefaultAsync(dd => dd.IdDefD == id && dd.IsActive, ct);

    public async Task<DefinicionDetalle> CreateDetalleAsync(DefinicionDetalle detalle, CancellationToken ct = default)
    {
        context.DefinicionDetalles.Add(detalle);
        await context.SaveChangesAsync(ct);
        return detalle;
    }

    public async Task<DefinicionDetalle> UpdateDetalleAsync(DefinicionDetalle detalle, CancellationToken ct = default)
    {
        context.DefinicionDetalles.Update(detalle);
        await context.SaveChangesAsync(ct);
        return detalle;
    }

    public async Task DeleteDetalleAsync(int id, CancellationToken ct = default) =>
        await context.DefinicionDetalles
            .Where(dd => dd.IdDefD == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(dd => dd.IsActive, false)
                .SetProperty(dd => dd.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> DetalleExistsAsync(int id, CancellationToken ct = default) =>
        await context.DefinicionDetalles.AnyAsync(dd => dd.IdDefD == id && dd.IsActive, ct);
}
