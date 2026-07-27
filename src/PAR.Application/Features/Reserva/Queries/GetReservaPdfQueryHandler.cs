using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Ports;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Queries;

public class GetReservaPdfQueryHandler(
    IReservaRepository reservaRepo,
    IReservaPdfService pdfService)
    : IRequestHandler<GetReservaPdfQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(GetReservaPdfQuery r, CancellationToken ct)
    {
        if (!await reservaRepo.ExistsAsync(r.ICodReserva, ct))
            return Result<byte[]>.Failure("Reserva no encontrada.", 404);

        var bytes = await pdfService.GenerarAsync(r.ICodReserva, ct);
        return Result<byte[]>.Success(bytes);
    }
}
