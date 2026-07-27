using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Venta.Queries;

public class GetVentasQueryHandler(IVentaRepository repo)
    : IRequestHandler<GetVentasQuery, Result<IEnumerable<VentaDto>>>
{
    public async Task<Result<IEnumerable<VentaDto>>> Handle(GetVentasQuery r, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<VentaDto>>.Success(list.Select(VentaDto.FromEntity));
    }
}
