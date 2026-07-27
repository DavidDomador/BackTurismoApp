using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public record UpdateReservaDetalleCommand(
    int Id, int ICodCliente, int? EstadoCliente, int? UserId)
    : IRequest<Result<ReservaDetalleDto>>;
