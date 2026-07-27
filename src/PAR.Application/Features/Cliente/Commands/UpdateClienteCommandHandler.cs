using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Cliente.Commands;

public class UpdateClienteCommandHandler(IClienteRepository repo)
    : IRequestHandler<UpdateClienteCommand, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> Handle(UpdateClienteCommand r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<ClienteDto>.Failure("Cliente no encontrado.", 404);

        entity.CNombres        = r.CNombres;
        entity.CApellidos      = r.CApellidos;
        entity.CDni            = r.CDni;
        entity.CCorreo         = r.CCorreo;
        entity.CEdad           = r.CEdad;
        entity.CDireccion      = r.CDireccion;
        entity.UpdatedByUserId = r.UserId;
        entity.UpdatedAt       = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(entity, ct);
        return Result<ClienteDto>.Success(ClienteDto.FromEntity(updated));
    }
}
