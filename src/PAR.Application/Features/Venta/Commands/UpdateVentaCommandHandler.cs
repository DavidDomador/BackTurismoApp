using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Venta.Commands;

public class UpdateVentaCommandHandler(
    IVentaRepository repo,
    IReservaRepository reservaRepo,
    IReservaDetalleRepository reservaDetalleRepo)
    : IRequestHandler<UpdateVentaCommand, Result<VentaDto>>
{
    public async Task<Result<VentaDto>> Handle(UpdateVentaCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<VentaDto>.Failure("Venta no encontrada.", 404);

        var reserva = await reservaRepo.GetByIdAsync(r.ICodReserva, ct);
        if (reserva is null) return Result<VentaDto>.Failure("Reserva no encontrada.", 404);

        var detalles    = await reservaDetalleRepo.GetByReservaAsync(r.ICodReserva, ct);
        decimal tarifa  = reserva.RTarifa ?? 0m;
        decimal vTotal  = tarifa * detalles.Count(d => d.IsActive);

        entity.ICodReserva     = r.ICodReserva;
        entity.VFechaVenta     = r.VFechaVenta;
        entity.ICodUsuario     = r.ICodUsuario;
        entity.VUsuarioNombre  = r.VUsuarioNombre;
        entity.ICodCliente     = r.ICodCliente;
        entity.VTotal          = vTotal;
        entity.VSaldo          = r.VSaldo;
        entity.UpdatedByUserId = r.UserId;
        entity.UpdatedAt       = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<VentaDto>.Success(VentaDto.FromEntity(updated));
    }
}
