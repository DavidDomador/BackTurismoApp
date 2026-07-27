using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Ports;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Queries;

public class GetPaquetePdfQueryHandler(
    IPaqueteRepository paqueteRepo,
    IPaquetePdfService pdfService)
    : IRequestHandler<GetPaquetePdfQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(GetPaquetePdfQuery r, CancellationToken ct)
    {
        if (!await paqueteRepo.ExistsAsync(r.ICodPaquete, ct))
            return Result<byte[]>.Failure("Paquete no encontrado.", 404);

        var bytes = await pdfService.GenerarAsync(r.ICodPaquete, ct);
        return Result<byte[]>.Success(bytes);
    }
}
