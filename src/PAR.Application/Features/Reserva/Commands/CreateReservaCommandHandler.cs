using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Commands;

public class CreateReservaCommandHandler(IReservaRepository repo, IPaqueteRepository paqueteRepo)
    : IRequestHandler<CreateReservaCommand, Result<ReservaDto>>
{
    public async Task<Result<ReservaDto>> Handle(CreateReservaCommand r, CancellationToken ct)
    {
        if (!await paqueteRepo.ExistsAsync(r.ICodPaquete, ct))
            return Result<ReservaDto>.Failure("Paquete no encontrado.", 404);

        var numero = await repo.GetNextNumeroReservaAsync(ct);

        var entity = new Domain.Entities.Reserva
        {
            RNumeroReserva  = numero,
            ICodPaquete     = r.ICodPaquete,
            FechaTour       = r.FechaTour,
            RTarifa         = r.RTarifa,
            RTotal          = 0,
            RAbono          = r.RAbono,
            RIncluye        = r.RIncluye,
            RNoIncluye      = r.RNoIncluye,
            RObservacion    = r.RObservacion,
            Estado          = r.Estado,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };

        var created = await repo.CreateAsync(entity, ct);
        return Result<ReservaDto>.Success(ReservaDto.FromEntity(created));
    }
}
