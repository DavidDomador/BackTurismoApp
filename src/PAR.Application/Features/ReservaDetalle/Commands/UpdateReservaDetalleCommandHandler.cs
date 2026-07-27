using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public class UpdateReservaDetalleCommandHandler(
    IReservaDetalleRepository repo,
    IClienteRepository clienteRepo)
    : IRequestHandler<UpdateReservaDetalleCommand, Result<ReservaDetalleDto>>
{
    public async Task<Result<ReservaDetalleDto>> Handle(UpdateReservaDetalleCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<ReservaDetalleDto>.Failure("Detalle no encontrado.", 404);

        if (!await clienteRepo.ExistsAsync(r.ICodCliente, ct))
            return Result<ReservaDetalleDto>.Failure("Cliente no encontrado.", 404);

        entity.ICodCliente     = r.ICodCliente;
        entity.EstadoCliente   = r.EstadoCliente;
        entity.UpdatedByUserId = r.UserId;
        entity.UpdatedAt       = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<ReservaDetalleDto>.Success(ReservaDetalleDto.FromEntity(updated));
    }
}
