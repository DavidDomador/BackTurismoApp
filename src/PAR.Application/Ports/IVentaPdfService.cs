namespace PAR.Application.Ports;

public interface IVentaPdfService
{
    Task<byte[]> GenerarAsync(int iCodVenta, CancellationToken ct = default);
}
