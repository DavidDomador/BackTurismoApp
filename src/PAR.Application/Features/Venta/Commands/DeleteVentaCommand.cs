using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Venta.Commands;

public record DeleteVentaCommand(int Id) : IRequest<Result<bool>>;
