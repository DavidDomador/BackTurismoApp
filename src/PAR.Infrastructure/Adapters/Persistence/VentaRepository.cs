using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class VentaRepository(AppDbContext context) : IVentaRepository
{
    public async Task<IEnumerable<Venta>> GetAllAsync(CancellationToken ct = default) =>
        await context.Ventas
            .Include(v => v.Reserva).ThenInclude(r => r!.Paquete)
            .Include(v => v.Cliente)
            .Include(v => v.Detalles!).ThenInclude(d => d.Cliente)
            .Where(v => v.IsActive)
            .OrderByDescending(v => v.ICodVenta)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Venta?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Ventas
            .Include(v => v.Reserva).ThenInclude(r => r!.Paquete)
            .Include(v => v.Cliente)
            .Include(v => v.Detalles!).ThenInclude(d => d.Cliente)
            .FirstOrDefaultAsync(v => v.ICodVenta == id && v.IsActive, ct);

    public async Task<Venta> CreateAsync(Venta venta, CancellationToken ct = default)
    {
        context.Ventas.Add(venta);
        await context.SaveChangesAsync(ct);
        // Generar número de venta después de obtener el PK
        venta.VNumeroVenta = "P" + venta.ICodVenta.ToString("D6");
        await context.SaveChangesAsync(ct);
        await context.Entry(venta).Reference(v => v.Reserva).LoadAsync(ct);
        if (venta.Reserva is not null)
            await context.Entry(venta.Reserva).Reference(r => r.Paquete).LoadAsync(ct);
        await context.Entry(venta).Reference(v => v.Cliente).LoadAsync(ct);
        return venta;
    }

    public async Task<Venta> UpdateAsync(Venta venta, CancellationToken ct = default)
    {
        context.Ventas.Update(venta);
        await context.SaveChangesAsync(ct);
        await context.Entry(venta).Reference(v => v.Reserva).LoadAsync(ct);
        if (venta.Reserva is not null)
            await context.Entry(venta.Reserva).Reference(r => r.Paquete).LoadAsync(ct);
        await context.Entry(venta).Reference(v => v.Cliente).LoadAsync(ct);
        return venta;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) =>
        await context.Ventas
            .Where(v => v.ICodVenta == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(v => v.IsActive, false)
                .SetProperty(v => v.UpdatedAt, DateTime.UtcNow), ct);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Ventas.AnyAsync(v => v.ICodVenta == id && v.IsActive, ct);

    /// <summary>
    /// Busca el cliente con estado "encargado de grupo" (DdValue contiene "encargado")
    /// en los detalles activos de la reserva indicada.
    /// </summary>
    public async Task<int?> GetEncargadoGrupoClienteAsync(int iCodReserva, CancellationToken ct = default) =>
        await context.ReservaDetalles
            .Where(d => d.ICodReserva == iCodReserva && d.IsActive)
            .Join(context.DefinicionDetalles,
                d => d.EstadoCliente,
                def => def.IdDefD,
                (d, def) => new { d.ICodCliente, def.DdValue })
            .Where(x => x.DdValue.ToLower().Contains("encargado"))
            .Select(x => (int?)x.ICodCliente)
            .FirstOrDefaultAsync(ct);
}
