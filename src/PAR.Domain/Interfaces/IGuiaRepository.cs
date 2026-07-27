using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IGuiaRepository
{
    Task<IEnumerable<Guia>> GetAllAsync(CancellationToken ct = default);
    Task<Guia?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Guia> CreateAsync(Guia guia, CancellationToken ct = default);
    Task<Guia> UpdateAsync(Guia guia, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
