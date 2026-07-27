using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IEmpresaRepository
{
    Task<IEnumerable<Empresa>> GetAllAsync(CancellationToken ct = default);
    Task<Empresa?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Empresa> CreateAsync(Empresa empresa, CancellationToken ct = default);
    Task<Empresa> UpdateAsync(Empresa empresa, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
