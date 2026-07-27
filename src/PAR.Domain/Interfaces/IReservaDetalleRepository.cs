using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IReservaDetalleRepository
{
    Task<IEnumerable<ReservaDetalle>> GetByReservaAsync(int iCodReserva, CancellationToken ct = default);
    Task<ReservaDetalle?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ReservaDetalle> CreateAsync(ReservaDetalle detalle, CancellationToken ct = default);
    Task<ReservaDetalle> UpdateAsync(ReservaDetalle detalle, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
