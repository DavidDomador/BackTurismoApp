namespace PAR.Application.Ports;

public interface ISalidaExcelService
{
    Task<byte[]> GenerarAsync(CancellationToken ct = default);
}
