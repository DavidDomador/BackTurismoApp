using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Guia.Queries;

public class GetGuiasQueryHandler(IGuiaRepository repo) : IRequestHandler<GetGuiasQuery, Result<IEnumerable<GuiaDto>>>
{
    public async Task<Result<IEnumerable<GuiaDto>>> Handle(GetGuiasQuery request, CancellationToken ct)
    {
        var guias = await repo.GetAllAsync(ct);
        return Result<IEnumerable<GuiaDto>>.Success(guias.Select(Map));
    }

    private static GuiaDto Map(Domain.Entities.Guia g) => new()
    {
        ICodigoGuia = g.ICodigoGuia,
        GNombre     = g.GNombre,
        GApellidos  = g.GApellidos,
        GDni        = g.GDni,
        GCorreo     = g.GCorreo,
        CreatedAt   = g.CreatedAt,
        UpdatedAt   = g.UpdatedAt,
        IsActive    = g.IsActive
    };
}
