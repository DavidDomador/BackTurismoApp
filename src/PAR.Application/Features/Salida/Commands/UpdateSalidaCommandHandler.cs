using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Salida.Commands;

public class UpdateSalidaCommandHandler(ISalidaRepository repo)
    : IRequestHandler<UpdateSalidaCommand, Result<SalidaDto>>
{
    public async Task<Result<SalidaDto>> Handle(UpdateSalidaCommand r, CancellationToken ct)
    {
        var ids = r.ReservaIds.Distinct().ToList();
        if (ids.Count == 0)
            return Result<SalidaDto>.Failure("Debe seleccionar al menos una reserva.", 400);

        var salida = await repo.GetByIdAsync(r.ICodSalida, ct);
        if (salida is null)
            return Result<SalidaDto>.Failure("Salida no encontrada.", 404);

        salida.ICodigoGuia        = r.ICodigoGuia;
        salida.ICodChoferVehiculo = r.ICodChoferVehiculo;
        salida.FechaSalida        = r.FechaSalida;
        salida.HoraSalida         = r.HoraSalida;
        salida.UpdatedByUserId    = r.UserId;
        salida.UpdatedAt          = DateTime.UtcNow;

        var newReservas = ids.Select(id => new SalidaReserva
        {
            ICodSalida  = salida.ICodSalida,
            ICodReserva = id
        }).ToList();

        var updated = await repo.UpdateAsync(salida, newReservas, ct);
        var full    = await repo.GetByIdAsync(updated.ICodSalida, ct);
        return Result<SalidaDto>.Success(SalidaDto.FromEntity(full!));
    }
}
