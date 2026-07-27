using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class UpdateDefinicionDetalleCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<UpdateDefinicionDetalleCommand, Result<DefinicionDetalleDto>>
{
    public async Task<Result<DefinicionDetalleDto>> Handle(UpdateDefinicionDetalleCommand r, CancellationToken ct)
    {
        var entity = await repo.GetDetalleByIdAsync(r.Id, ct);
        if (entity is null) return Result<DefinicionDetalleDto>.Failure("Detalle no encontrado.", 404);

        entity.DdValue       = r.DdValue;
        entity.DdAbreviacion = r.DdAbreviacion;
        entity.DdDescripcion = r.DdDescripcion;
        entity.Estado        = r.Estado;
        entity.UpdatedAt     = DateTime.UtcNow;
        entity.UpdatedByUserId = r.UserId;

        var updated = await repo.UpdateDetalleAsync(entity, ct);
        return Result<DefinicionDetalleDto>.Success(DefinicionDetalleDto.FromEntity(updated));
    }
}
