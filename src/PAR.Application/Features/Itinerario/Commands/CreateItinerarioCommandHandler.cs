using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Itinerario.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Itinerario.Commands;

public class CreateItinerarioCommandHandler(IItinerarioRepository repo)
    : IRequestHandler<CreateItinerarioCommand, Result<ItinerarioDto>>
{
    public async Task<Result<ItinerarioDto>> Handle(CreateItinerarioCommand r, CancellationToken ct)
    {
        var entity = new Domain.Entities.Itinerario
        {
            ICodPaquete     = r.ICodPaquete,
            INombre         = r.INombre,
            IDescripcion    = r.IDescripcion,
            Icon            = r.Icon,
            Imagen          = r.Imagen,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };
        var created = await repo.CreateAsync(entity, ct);
        return Result<ItinerarioDto>.Success(ItinerarioDto.FromEntity(created));
    }
}
