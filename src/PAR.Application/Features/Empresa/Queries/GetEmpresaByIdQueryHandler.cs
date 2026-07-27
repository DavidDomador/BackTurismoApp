using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Empresa.Queries;

public class GetEmpresaByIdQueryHandler(IEmpresaRepository repo)
    : IRequestHandler<GetEmpresaByIdQuery, Result<EmpresaDto>>
{
    public async Task<Result<EmpresaDto>> Handle(GetEmpresaByIdQuery r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        return entity is null
            ? Result<EmpresaDto>.Failure("Empresa no encontrada.", 404)
            : Result<EmpresaDto>.Success(EmpresaDto.FromEntity(entity));
    }
}
