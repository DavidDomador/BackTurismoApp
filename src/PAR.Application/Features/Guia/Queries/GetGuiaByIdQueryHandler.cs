using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Guia.Queries;

public class GetGuiaByIdQueryHandler(IGuiaRepository repo) : IRequestHandler<GetGuiaByIdQuery, Result<GuiaDto>>
{
    public async Task<Result<GuiaDto>> Handle(GetGuiaByIdQuery request, CancellationToken ct)
    {
        var g = await repo.GetByIdAsync(request.Id, ct);
        if (g is null) return Result<GuiaDto>.Failure("Guia not found.", 404);
        return Result<GuiaDto>.Success(new GuiaDto
        {
            ICodigoGuia = g.ICodigoGuia,
            GNombre     = g.GNombre,
            GApellidos  = g.GApellidos,
            GDni        = g.GDni,
            GCorreo     = g.GCorreo,
            CreatedAt   = g.CreatedAt,
            UpdatedAt   = g.UpdatedAt,
            IsActive    = g.IsActive
        });
    }
}
