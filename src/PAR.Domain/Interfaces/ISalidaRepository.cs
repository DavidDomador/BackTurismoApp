using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface ISalidaRepository
{
    Task<IEnumerable<Salida>> GetAllAsync(CancellationToken ct = default);
    Task<Salida?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Salida> CreateAsync(Salida salida, CancellationToken ct = default);
    Task<Salida> UpdateAsync(Salida salida, IList<SalidaReserva> newReservas, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
