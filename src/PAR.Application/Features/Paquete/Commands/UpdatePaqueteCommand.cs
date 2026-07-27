using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;

namespace PAR.Application.Features.Paquete.Commands;

public record UpdatePaqueteCommand(
    int Id,
    string PNombre,
    string? PDescripcion,
    decimal PPrecioBase,
    decimal? PPrecioDescuento,
    decimal? PPrecioReserva,
    int? UserId
) : IRequest<Result<PaqueteDto>>;
