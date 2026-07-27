using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Guia.Commands;

public class UpdateGuiaCommandHandler(IGuiaRepository repo) : IRequestHandler<UpdateGuiaCommand, Result<GuiaDto>>
{
    public async Task<Result<GuiaDto>> Handle(UpdateGuiaCommand request, CancellationToken ct)
    {
        var guia = await repo.GetByIdAsync(request.Id, ct);
        if (guia is null) return Result<GuiaDto>.Failure("Guia not found.", 404);

        guia.GNombre         = request.GNombre;
        guia.GApellidos      = request.GApellidos;
        guia.GDni            = request.GDni;
        guia.GCorreo         = request.GCorreo;
        guia.IsActive        = request.IsActive;
        guia.UpdatedAt       = DateTime.UtcNow;
        guia.UpdatedByUserId = request.UserId;

        await repo.UpdateAsync(guia, ct);
        return Result<GuiaDto>.Success(new GuiaDto
        {
            ICodigoGuia = guia.ICodigoGuia,
            GNombre     = guia.GNombre,
            GApellidos  = guia.GApellidos,
            GDni        = guia.GDni,
            GCorreo     = guia.GCorreo,
            CreatedAt   = guia.CreatedAt,
            UpdatedAt   = guia.UpdatedAt,
            IsActive    = guia.IsActive
        });
    }
}
