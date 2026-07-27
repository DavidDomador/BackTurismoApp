using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Cliente.Commands;

public class CreateClienteCommandHandler(IClienteRepository repo)
    : IRequestHandler<CreateClienteCommand, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> Handle(CreateClienteCommand r, CancellationToken ct)
    {
        var entity = new Domain.Entities.Cliente
        {
            CNombres        = r.CNombres,
            CApellidos      = r.CApellidos,
            CDni            = r.CDni,
            CCorreo         = r.CCorreo,
            CEdad           = r.CEdad,
            CDireccion      = r.CDireccion,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };
        var created = await repo.CreateAsync(entity, ct);
        return Result<ClienteDto>.Success(ClienteDto.FromEntity(created));
    }
}
