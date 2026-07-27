using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;

namespace PAR.Application.Features.Reserva.Queries;

public record GetReservasQuery : IRequest<Result<IEnumerable<ReservaDto>>>;
