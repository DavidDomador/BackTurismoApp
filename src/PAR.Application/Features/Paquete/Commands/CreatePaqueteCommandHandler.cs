using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Commands;

public class CreatePaqueteCommandHandler(IPaqueteRepository repo)
    : IRequestHandler<CreatePaqueteCommand, Result<PaqueteDto>>
{
    public async Task<Result<PaqueteDto>> Handle(CreatePaqueteCommand r, CancellationToken ct)
    {
        var entity = new Domain.Entities.Paquete
        {
            PNombre          = r.PNombre,
            PDescripcion     = r.PDescripcion,
            PPrecioBase      = r.PPrecioBase,
            PPrecioDescuento = r.PPrecioDescuento,
            PPrecioReserva   = r.PPrecioReserva,
            CreatedByUserId  = r.UserId,
            IsActive         = true
        };
        var created = await repo.CreateAsync(entity, ct);
        return Result<PaqueteDto>.Success(PaqueteDto.FromEntity(created));
    }
}
