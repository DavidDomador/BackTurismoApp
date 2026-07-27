using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Empresa.Commands;

public class CreateEmpresaCommandHandler(IEmpresaRepository repo)
    : IRequestHandler<CreateEmpresaCommand, Result<EmpresaDto>>
{
    public async Task<Result<EmpresaDto>> Handle(CreateEmpresaCommand r, CancellationToken ct)
    {
        // Singleton: solo se permite un registro de empresa
        var existing = await repo.GetAllAsync(ct);
        if (existing.Any())
            return Result<EmpresaDto>.Failure("Ya existe un registro de empresa. Solo se permite uno.", 409);

        var entity = new Domain.Entities.Empresa
        {
            ERazonSocial    = r.ERazonSocial,
            ERUC            = r.ERUC,
            EDireccion      = r.EDireccion,
            ETelefono       = r.ETelefono,
            ECorreo         = r.ECorreo,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };
        var created = await repo.CreateAsync(entity, ct);
        return Result<EmpresaDto>.Success(EmpresaDto.FromEntity(created));
    }
}
