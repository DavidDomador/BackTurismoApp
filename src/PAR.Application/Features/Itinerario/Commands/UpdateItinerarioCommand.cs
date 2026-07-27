using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Itinerario.DTOs;

namespace PAR.Application.Features.Itinerario.Commands;

public record UpdateItinerarioCommand(
    int Id, string INombre, string? IDescripcion,
    string? Icon, string? Imagen, int? UserId
) : IRequest<Result<ItinerarioDto>>;
