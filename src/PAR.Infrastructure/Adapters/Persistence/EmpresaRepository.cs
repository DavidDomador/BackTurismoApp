using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class EmpresaRepository(AppDbContext context) : IEmpresaRepository
{
    public async Task<IEnumerable<Empresa>> GetAllAsync(CancellationToken ct = default) =>
        await context.Empresas
            .Where(e => e.IsActive)
            .OrderBy(e => e.ERazonSocial)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Empresa?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Empresas
            .FirstOrDefaultAsync(e => e.ICodEmpresa == id && e.IsActive, ct);

    public async Task<Empresa> CreateAsync(Empresa empresa, CancellationToken ct = default)
    {
        context.Empresas.Add(empresa);
        await context.SaveChangesAsync(ct);
        return empresa;
    }

    public async Task<Empresa> UpdateAsync(Empresa empresa, CancellationToken ct = default)
    {
        context.Empresas.Update(empresa);
        await context.SaveChangesAsync(ct);
        return empresa;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Empresas
            .Where(e => e.ICodEmpresa == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(e => e.IsActive, false)
                .SetProperty(e => e.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Empresas.AnyAsync(e => e.ICodEmpresa == id && e.IsActive, ct);
}
