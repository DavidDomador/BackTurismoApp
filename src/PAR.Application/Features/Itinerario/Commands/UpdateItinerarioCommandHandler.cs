using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Itinerario.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Itinerario.Commands;

public class UpdateItinerarioCommandHandler(IItinerarioRepository repo)
    : IRequestHandler<UpdateItinerarioCommand, Result<ItinerarioDto>>
{
    public async Task<Result<ItinerarioDto>> Handle(UpdateItinerarioCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<ItinerarioDto>.Failure("Itinerario no encontrado.", 404);

        entity.INombre          = r.INombre;
        entity.IDescripcion     = r.IDescripcion;
        entity.Icon             = r.Icon;
        entity.UpdatedAt        = DateTime.UtcNow;
        entity.UpdatedByUserId  = r.UserId;

        // Solo actualizar imagen si se proveyó una nueva
        if (r.Imagen is not null)
            entity.Imagen = r.Imagen;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<ItinerarioDto>.Success(ItinerarioDto.FromEntity(updated));
    }
}
