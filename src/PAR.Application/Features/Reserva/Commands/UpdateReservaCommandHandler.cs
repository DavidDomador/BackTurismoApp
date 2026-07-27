using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Commands;

public class UpdateReservaCommandHandler(IReservaRepository repo, IPaqueteRepository paqueteRepo)
    : IRequestHandler<UpdateReservaCommand, Result<ReservaDto>>
{
    public async Task<Result<ReservaDto>> Handle(UpdateReservaCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<ReservaDto>.Failure("Reserva no encontrada.", 404);

        if (!await paqueteRepo.ExistsAsync(r.ICodPaquete, ct))
            return Result<ReservaDto>.Failure("Paquete no encontrado.", 404);

        entity.ICodPaquete     = r.ICodPaquete;
        entity.FechaTour       = r.FechaTour;
        entity.RTarifa         = r.RTarifa;
        entity.RAbono          = r.RAbono;
        entity.RIncluye        = r.RIncluye;
        entity.RNoIncluye      = r.RNoIncluye;
        entity.RObservacion    = r.RObservacion;
        entity.Estado          = r.Estado;
        entity.UpdatedByUserId = r.UserId;
        entity.UpdatedAt       = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<ReservaDto>.Success(ReservaDto.FromEntity(updated));
    }
}
