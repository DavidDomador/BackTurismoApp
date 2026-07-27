using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Commands;

public class UpdatePaqueteCommandHandler(IPaqueteRepository repo)
    : IRequestHandler<UpdatePaqueteCommand, Result<PaqueteDto>>
{
    public async Task<Result<PaqueteDto>> Handle(UpdatePaqueteCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<PaqueteDto>.Failure("Paquete no encontrado.", 404);

        entity.PNombre          = r.PNombre;
        entity.PDescripcion     = r.PDescripcion;
        entity.PPrecioBase      = r.PPrecioBase;
        entity.PPrecioDescuento = r.PPrecioDescuento;
        entity.PPrecioReserva   = r.PPrecioReserva;
        entity.UpdatedAt        = DateTime.UtcNow;
        entity.UpdatedByUserId  = r.UserId;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<PaqueteDto>.Success(PaqueteDto.FromEntity(updated));
    }
}
