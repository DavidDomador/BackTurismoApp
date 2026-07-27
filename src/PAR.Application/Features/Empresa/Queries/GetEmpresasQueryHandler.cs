using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Empresa.Queries;

public class GetEmpresasQueryHandler(IEmpresaRepository repo)
    : IRequestHandler<GetEmpresasQuery, Result<IEnumerable<EmpresaDto>>>
{
    public async Task<Result<IEnumerable<EmpresaDto>>> Handle(GetEmpresasQuery r, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<EmpresaDto>>.Success(list.Select(EmpresaDto.FromEntity));
    }
}
