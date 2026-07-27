using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Reserva.Queries;

public record GetReservaPdfQuery(int ICodReserva) : IRequest<Result<byte[]>>;
