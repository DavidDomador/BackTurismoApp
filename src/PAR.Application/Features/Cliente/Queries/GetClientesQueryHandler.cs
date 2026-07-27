using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Cliente.Queries;

public class GetClientesQueryHandler(IClienteRepository repo)
    : IRequestHandler<GetClientesQuery, Result<IEnumerable<ClienteDto>>>
{
    public async Task<Result<IEnumerable<ClienteDto>>> Handle(GetClientesQuery r, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<ClienteDto>>.Success(list.Select(ClienteDto.FromEntity));
    }
}
