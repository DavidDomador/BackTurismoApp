using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface ITransporteRepository
{
    Task<IEnumerable<ChoferVehiculo>> GetAllAsync(CancellationToken ct = default);
    Task<ChoferVehiculo?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ChoferVehiculo> CreateAsync(Chofer chofer, Vehiculo vehiculo, DateTime fechaAsignacion, int? userId, CancellationToken ct = default);
    Task<ChoferVehiculo> UpdateAsync(int id, Chofer chofer, Vehiculo vehiculo, int? userId, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
