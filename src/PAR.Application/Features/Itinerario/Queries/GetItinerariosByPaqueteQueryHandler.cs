using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Itinerario.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Itinerario.Queries;

public class GetItinerariosByPaqueteQueryHandler(IItinerarioRepository repo)
    : IRequestHandler<GetItinerariosByPaqueteQuery, Result<IEnumerable<ItinerarioDto>>>
{
    public async Task<Result<IEnumerable<ItinerarioDto>>> Handle(GetItinerariosByPaqueteQuery r, CancellationToken ct)
    {
        var list = await repo.GetByPaqueteAsync(r.ICodPaquete, ct);
        return Result<IEnumerable<ItinerarioDto>>.Success(list.Select(ItinerarioDto.FromEntity));
    }
}
