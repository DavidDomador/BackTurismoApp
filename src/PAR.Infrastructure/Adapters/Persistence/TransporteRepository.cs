using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class TransporteRepository(AppDbContext context) : ITransporteRepository
{
    private IQueryable<ChoferVehiculo> BaseQuery() =>
        context.ChoferVehiculos
            .Include(cv => cv.Chofer)
            .Include(cv => cv.Vehiculo)
            .Where(cv => cv.IsActive && cv.Chofer.IsActive && cv.Vehiculo.IsActive);

    public async Task<IEnumerable<ChoferVehiculo>> GetAllAsync(CancellationToken ct = default) =>
        await BaseQuery().OrderBy(cv => cv.Chofer.GApellidos).AsNoTracking().ToListAsync(ct);

    public async Task<ChoferVehiculo?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await BaseQuery().FirstOrDefaultAsync(cv => cv.ICodigoChoferVehiculo == id, ct);

    public async Task<ChoferVehiculo> CreateAsync(Chofer chofer, Vehiculo vehiculo, DateTime fechaAsignacion, int? userId, CancellationToken ct = default)
    {
        chofer.CreatedAt       = DateTime.UtcNow;
        chofer.CreatedByUserId = userId;
        vehiculo.CreatedAt     = DateTime.UtcNow;
        vehiculo.CreatedByUserId = userId;

        context.Choferes.Add(chofer);
        context.Vehiculos.Add(vehiculo);
        await context.SaveChangesAsync(ct);

        var cv = new ChoferVehiculo
        {
            ChoferID        = chofer.ICodigoChofer,
            VehiculoID      = vehiculo.ICodigoVehiculo,
            FechaAsignacion = fechaAsignacion,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow,
            CreatedByUserId = userId
        };
        context.ChoferVehiculos.Add(cv);
        await context.SaveChangesAsync(ct);

        cv.Chofer   = chofer;
        cv.Vehiculo = vehiculo;
        return cv;
    }

    public async Task<ChoferVehiculo> UpdateAsync(int id, Chofer choferData, Vehiculo vehiculoData, int? userId, CancellationToken ct = default)
    {
        var cv = await BaseQuery().FirstOrDefaultAsync(x => x.ICodigoChoferVehiculo == id, ct)
                 ?? throw new KeyNotFoundException($"ChoferVehiculo {id} not found.");

        cv.Chofer.GNombre    = choferData.GNombre;
        cv.Chofer.GApellidos = choferData.GApellidos;
        cv.Chofer.GDni       = choferData.GDni;
        cv.Chofer.GLicencia  = choferData.GLicencia;
        cv.Chofer.GTelefono  = choferData.GTelefono;
        cv.Chofer.UpdatedAt        = DateTime.UtcNow;
        cv.Chofer.UpdatedByUserId  = userId;

        cv.Vehiculo.GPlaca  = vehiculoData.GPlaca;
        cv.Vehiculo.GMarca  = vehiculoData.GMarca;
        cv.Vehiculo.GModelo = vehiculoData.GModelo;
        cv.Vehiculo.GAnio   = vehiculoData.GAnio;
        cv.Vehiculo.GColor  = vehiculoData.GColor;
        cv.Vehiculo.GCantidadAsientos = vehiculoData.GCantidadAsientos;
        cv.Vehiculo.UpdatedAt       = DateTime.UtcNow;
        cv.Vehiculo.UpdatedByUserId = userId;

        await context.SaveChangesAsync(ct);
        return cv;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await context.ChoferVehiculos.Where(cv => cv.ICodigoChoferVehiculo == id)
            .ExecuteUpdateAsync(s => s.SetProperty(cv => cv.IsActive, false), ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.ChoferVehiculos.AnyAsync(cv => cv.ICodigoChoferVehiculo == id && cv.IsActive, ct);
}
