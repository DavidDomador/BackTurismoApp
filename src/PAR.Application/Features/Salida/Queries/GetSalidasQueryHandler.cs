using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Salida.Queries;

public class GetSalidasQueryHandler(ISalidaRepository repo)
    : IRequestHandler<GetSalidasQuery, Result<IEnumerable<SalidaDto>>>
{
    public async Task<Result<IEnumerable<SalidaDto>>> Handle(GetSalidasQuery request, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<SalidaDto>>.Success(list.Select(SalidaDto.FromEntity));
    }
}
