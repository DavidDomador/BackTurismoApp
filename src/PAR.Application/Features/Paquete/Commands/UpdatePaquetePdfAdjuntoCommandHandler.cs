using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Commands;

public class UpdatePaquetePdfAdjuntoCommandHandler(IPaqueteRepository repo)
    : IRequestHandler<UpdatePaquetePdfAdjuntoCommand, Result<PaqueteDto>>
{
    public async Task<Result<PaqueteDto>> Handle(UpdatePaquetePdfAdjuntoCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null)
            return Result<PaqueteDto>.Failure("Paquete no encontrado.", 404);

        entity.PdfAdjunto = r.PdfAdjunto;
        entity.UpdatedAt  = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<PaqueteDto>.Success(PaqueteDto.FromEntity(updated));
    }
}
