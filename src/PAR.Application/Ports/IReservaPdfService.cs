namespace PAR.Application.Ports;

/// <summary>Puerto de salida para la generación del voucher PDF de una reserva.</summary>
public interface IReservaPdfService
{
    Task<byte[]> GenerarAsync(int iCodReserva, CancellationToken ct = default);
}
