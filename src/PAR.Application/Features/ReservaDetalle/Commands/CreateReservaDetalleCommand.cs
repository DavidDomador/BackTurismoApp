using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public record CreateReservaDetalleCommand(
    int ICodReserva, int ICodCliente, int? EstadoCliente, int? UserId)
    : IRequest<Result<ReservaDetalleDto>>;
