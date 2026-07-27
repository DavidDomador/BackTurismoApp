using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Empresa.Commands;

public class UpdateEmpresaCommandHandler(IEmpresaRepository repo)
    : IRequestHandler<UpdateEmpresaCommand, Result<EmpresaDto>>
{
    public async Task<Result<EmpresaDto>> Handle(UpdateEmpresaCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<EmpresaDto>.Failure("Empresa no encontrada.", 404);

        entity.ERazonSocial    = r.ERazonSocial;
        entity.ERUC            = r.ERUC;
        entity.EDireccion      = r.EDireccion;
        entity.ETelefono       = r.ETelefono;
        entity.ECorreo         = r.ECorreo;
        entity.UpdatedByUserId = r.UserId;
        entity.UpdatedAt       = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<EmpresaDto>.Success(EmpresaDto.FromEntity(updated));
    }
}
