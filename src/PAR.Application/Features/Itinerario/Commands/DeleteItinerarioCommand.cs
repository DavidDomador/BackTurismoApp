using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Itinerario.Commands;

public record DeleteItinerarioCommand(int Id) : IRequest<Result<bool>>;
