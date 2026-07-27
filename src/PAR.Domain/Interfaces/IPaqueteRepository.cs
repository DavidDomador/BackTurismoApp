using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IPaqueteRepository
{
    Task<IEnumerable<Paquete>> GetAllAsync(CancellationToken ct = default);
    Task<Paquete?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Paquete> CreateAsync(Paquete paquete, CancellationToken ct = default);
    Task<Paquete> UpdateAsync(Paquete paquete, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
