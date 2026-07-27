using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;

namespace PAR.Application.Features.Venta.Commands;

public record CreateVentaCommand(
    int ICodReserva,
    DateTime VFechaVenta,
    int ICodUsuario,
    string VUsuarioNombre,
    int? ICodCliente,
    decimal? VSaldo,
    int? UserId)
    : IRequest<Result<VentaDto>>;
