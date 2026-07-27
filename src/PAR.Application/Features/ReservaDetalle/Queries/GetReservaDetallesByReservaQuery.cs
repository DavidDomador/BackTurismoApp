using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;

namespace PAR.Application.Features.ReservaDetalle.Queries;

public record GetReservaDetallesByReservaQuery(int ICodReserva)
    : IRequest<Result<IEnumerable<ReservaDetalleDto>>>;
