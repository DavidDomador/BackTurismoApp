using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IItinerarioRepository
{
    Task<IEnumerable<Itinerario>> GetByPaqueteAsync(int iCodPaquete, CancellationToken ct = default);
    Task<Itinerario?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Itinerario> CreateAsync(Itinerario itinerario, CancellationToken ct = default);
    Task<Itinerario> UpdateAsync(Itinerario itinerario, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
