using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Venta.Commands;

public class CreateVentaCommandHandler(
    IVentaRepository repo,
    IVentaDetalleRepository detalleRepo,
    IReservaRepository reservaRepo,
    IReservaDetalleRepository reservaDetalleRepo)
    : IRequestHandler<CreateVentaCommand, Result<VentaDto>>
{
    public async Task<Result<VentaDto>> Handle(CreateVentaCommand r, CancellationToken ct)
    {
        // Verificar reserva y cargar sus pasajeros
        var reserva = await reservaRepo.GetByIdAsync(r.ICodReserva, ct);
        if (reserva is null)
            return Result<VentaDto>.Failure("Reserva no encontrada.", 404);

        // Calcular saldo dinámicamente: total = tarifa × pasajeros, saldo = total − abono
        var reservaDetalles = await reservaDetalleRepo.GetByReservaAsync(r.ICodReserva, ct);
        var activeDetalles  = reservaDetalles.Where(d => d.IsActive).ToList();
        decimal tarifa      = reserva.RTarifa ?? 0m;
        decimal rTotal      = tarifa * activeDetalles.Count;
        decimal rSaldo      = rTotal - (reserva.RAbono ?? 0m);

        // Validación: si el saldo calculado > 0, el VSaldo es requerido y no puede ser igual al saldo
        if (rSaldo > 0)
        {
            if (r.VSaldo is null)
                return Result<VentaDto>.Failure("Se requiere ingresar el saldo de la venta porque la reserva tiene saldo pendiente.", 400);

            if (r.VSaldo.Value == rSaldo)
                return Result<VentaDto>.Failure(
                    $"El saldo de la venta (S/ {r.VSaldo.Value:N2}) no puede ser igual al saldo pendiente de la reserva (S/ {rSaldo:N2}). Debe indicar el nuevo saldo restante.", 400);
        }

        var entity = new Domain.Entities.Venta
        {
            ICodReserva     = r.ICodReserva,
            VFechaVenta     = r.VFechaVenta,
            ICodUsuario     = r.ICodUsuario,
            VUsuarioNombre  = r.VUsuarioNombre,
            ICodCliente     = r.ICodCliente,
            VTotal          = rTotal,
            VSaldo          = rSaldo > 0 ? r.VSaldo : null,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };

        var created = await repo.CreateAsync(entity, ct);

        // Crear VentaDetalles desde los ReservaDetalles activos
        var detalles = activeDetalles
            .Select(d => new VentaDetalle
            {
                ICodVenta          = created.ICodVenta,
                ICodReservaDetalle = d.ICodReservaDetalle,
                ICodCliente        = d.ICodCliente,
                VMonto             = tarifa,
                IsActive           = true
            })
            .ToList();

        if (detalles.Any())
            await detalleRepo.CreateManyAsync(detalles, ct);

        // Recargar con detalles para el DTO
        var full = await repo.GetByIdAsync(created.ICodVenta, ct);
        return Result<VentaDto>.Success(VentaDto.FromEntity(full!));
    }
}
