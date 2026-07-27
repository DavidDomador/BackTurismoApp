using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Reserva.Commands;

public record DeleteReservaCommand(int Id) : IRequest<Result<bool>>;
