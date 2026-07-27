namespace PAR.Application.Ports;

/// <summary>Puerto de salida para la generación del PDF turístico de un paquete.</summary>
public interface IPaquetePdfService
{
    Task<byte[]> GenerarAsync(int iCodPaquete, CancellationToken ct = default);
}
