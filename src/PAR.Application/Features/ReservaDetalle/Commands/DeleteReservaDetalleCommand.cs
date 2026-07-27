using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public record DeleteReservaDetalleCommand(int Id) : IRequest<Result<bool>>;
