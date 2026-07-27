using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IVentaRepository
{
    Task<IEnumerable<Venta>> GetAllAsync(CancellationToken ct = default);
    Task<Venta?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Venta> CreateAsync(Venta venta, CancellationToken ct = default);
    Task<Venta> UpdateAsync(Venta venta, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<int?> GetEncargadoGrupoClienteAsync(int iCodReserva, CancellationToken ct = default);
}
