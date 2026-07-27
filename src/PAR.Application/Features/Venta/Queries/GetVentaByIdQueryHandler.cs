using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Venta.Queries;

public class GetVentaByIdQueryHandler(IVentaRepository repo)
    : IRequestHandler<GetVentaByIdQuery, Result<VentaDto>>
{
    public async Task<Result<VentaDto>> Handle(GetVentaByIdQuery r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        return entity is null
            ? Result<VentaDto>.Failure("Venta no encontrada.", 404)
            : Result<VentaDto>.Success(VentaDto.FromEntity(entity));
    }
}
