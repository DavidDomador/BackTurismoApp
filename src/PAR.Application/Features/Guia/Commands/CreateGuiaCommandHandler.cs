using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;
using PAR.Domain.Ports;
using GuiaEntity = PAR.Domain.Entities.Guia;

namespace PAR.Application.Features.Guia.Commands;

public class CreateGuiaCommandHandler(IGuiaRepository repo) : IRequestHandler<CreateGuiaCommand, Result<GuiaDto>>
{
    public async Task<Result<GuiaDto>> Handle(CreateGuiaCommand request, CancellationToken ct)
    {
        var guia = new GuiaEntity
        {
            GNombre         = request.GNombre,
            GApellidos      = request.GApellidos,
            GDni            = request.GDni,
            GCorreo         = request.GCorreo,
            CreatedAt       = DateTime.UtcNow,
            CreatedByUserId = request.UserId,
            IsActive        = true
        };
        await repo.CreateAsync(guia, ct);
        return Result<GuiaDto>.Success(new GuiaDto
        {
            ICodigoGuia = guia.ICodigoGuia,
            GNombre     = guia.GNombre,
            GApellidos  = guia.GApellidos,
            GDni        = guia.GDni,
            GCorreo     = guia.GCorreo,
            CreatedAt   = guia.CreatedAt,
            IsActive    = guia.IsActive
        });
    }
}
