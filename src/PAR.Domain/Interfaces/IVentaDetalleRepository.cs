using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IVentaDetalleRepository
{
    Task<IEnumerable<VentaDetalle>> GetByVentaAsync(int iCodVenta, CancellationToken ct = default);
    Task CreateManyAsync(IEnumerable<VentaDetalle> detalles, CancellationToken ct = default);
    Task DeleteByVentaAsync(int iCodVenta, CancellationToken ct = default);
}
