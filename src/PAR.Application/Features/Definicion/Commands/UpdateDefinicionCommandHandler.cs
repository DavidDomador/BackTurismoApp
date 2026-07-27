using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class UpdateDefinicionCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<UpdateDefinicionCommand, Result<DefinicionDto>>
{
    public async Task<Result<DefinicionDto>> Handle(UpdateDefinicionCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<DefinicionDto>.Failure("Definición no encontrada.", 404);

        entity.DNombre      = r.DNombre;
        entity.DDescripcion = r.DDescripcion;
        entity.Estado       = r.Estado;
        entity.UpdatedAt    = DateTime.UtcNow;
        entity.UpdatedByUserId = r.UserId;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<DefinicionDto>.Success(DefinicionDto.FromEntity(updated));
    }
}
