using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Itinerario.DTOs;

namespace PAR.Application.Features.Itinerario.Queries;

public record GetItinerariosByPaqueteQuery(int ICodPaquete) : IRequest<Result<IEnumerable<ItinerarioDto>>>;
