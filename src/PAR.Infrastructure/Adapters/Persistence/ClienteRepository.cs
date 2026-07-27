using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public async Task<IEnumerable<Cliente>> GetAllAsync(CancellationToken ct = default) =>
        await context.Clientes
            .Where(c => c.IsActive)
            .OrderBy(c => c.CApellidos).ThenBy(c => c.CNombres)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Clientes
            .FirstOrDefaultAsync(c => c.ICodCliente == id && c.IsActive, ct);

    public async Task<Cliente> CreateAsync(Cliente cliente, CancellationToken ct = default)
    {
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(ct);
        return cliente;
    }

    public async Task<Cliente> UpdateAsync(Cliente cliente, CancellationToken ct = default)
    {
        context.Clientes.Update(cliente);
        await context.SaveChangesAsync(ct);
        return cliente;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Clientes
            .Where(c => c.ICodCliente == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.IsActive, false)
                .SetProperty(c => c.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Clientes.AnyAsync(c => c.ICodCliente == id && c.IsActive, ct);
}
