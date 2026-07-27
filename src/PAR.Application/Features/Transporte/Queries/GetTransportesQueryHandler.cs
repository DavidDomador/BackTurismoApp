using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Transporte.Queries;

public class GetTransportesQueryHandler(ITransporteRepository repo) : IRequestHandler<GetTransportesQuery, Result<IEnumerable<TransporteDto>>>
{
    public async Task<Result<IEnumerable<TransporteDto>>> Handle(GetTransportesQuery request, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<TransporteDto>>.Success(list.Select(TransporteDto.FromEntity));
    }
}
