using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class CreateDefinicionDetalleCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<CreateDefinicionDetalleCommand, Result<DefinicionDetalleDto>>
{
    public async Task<Result<DefinicionDetalleDto>> Handle(CreateDefinicionDetalleCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.IdDef, ct))
            return Result<DefinicionDetalleDto>.Failure("Definición padre no encontrada.", 404);

        var entity = new Domain.Entities.DefinicionDetalle
        {
            IdDef         = r.IdDef,
            DdValue       = r.DdValue,
            DdAbreviacion = r.DdAbreviacion,
            DdDescripcion = r.DdDescripcion,
            Estado        = r.Estado,
            CreatedByUserId = r.UserId,
            IsActive      = true
        };
        var created = await repo.CreateDetalleAsync(entity, ct);
        return Result<DefinicionDetalleDto>.Success(DefinicionDetalleDto.FromEntity(created));
    }
}
