using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Salida.Commands;

public class CreateSalidaCommandHandler(ISalidaRepository repo)
    : IRequestHandler<CreateSalidaCommand, Result<SalidaDto>>
{
    public async Task<Result<SalidaDto>> Handle(CreateSalidaCommand r, CancellationToken ct)
    {
        var ids = r.ReservaIds.Distinct().ToList();
        if (ids.Count == 0)
            return Result<SalidaDto>.Failure("Debe seleccionar al menos una reserva.", 400);

        var entity = new Domain.Entities.Salida
        {
            ICodigoGuia        = r.ICodigoGuia,
            ICodChoferVehiculo = r.ICodChoferVehiculo,
            FechaSalida        = r.FechaSalida,
            HoraSalida         = r.HoraSalida,
            CreatedByUserId    = r.UserId,
            IsActive           = true,
            SalidaReservas     = ids.Select(id => new SalidaReserva { ICodReserva = id }).ToList()
        };

        var created = await repo.CreateAsync(entity, ct);
        var full    = await repo.GetByIdAsync(created.ICodSalida, ct);
        return Result<SalidaDto>.Success(SalidaDto.FromEntity(full!));
    }
}
