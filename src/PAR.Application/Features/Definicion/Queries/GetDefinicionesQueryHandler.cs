using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Queries;

public class GetDefinicionesQueryHandler(IDefinicionRepository repo)
    : IRequestHandler<GetDefinicionesQuery, Result<IEnumerable<DefinicionDto>>>
{
    public async Task<Result<IEnumerable<DefinicionDto>>> Handle(GetDefinicionesQuery request, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<DefinicionDto>>.Success(list.Select(DefinicionDto.FromEntity));
    }
}
