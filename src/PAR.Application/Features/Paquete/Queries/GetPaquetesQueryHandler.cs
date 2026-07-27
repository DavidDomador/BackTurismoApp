using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Queries;

public class GetPaquetesQueryHandler(IPaqueteRepository repo)
    : IRequestHandler<GetPaquetesQuery, Result<IEnumerable<PaqueteDto>>>
{
    public async Task<Result<IEnumerable<PaqueteDto>>> Handle(GetPaquetesQuery r, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<PaqueteDto>>.Success(list.Select(PaqueteDto.FromEntity));
    }
}
