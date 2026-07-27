using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.ReservaDetalle.Queries;

public class GetReservaDetallesByReservaQueryHandler(IReservaDetalleRepository repo)
    : IRequestHandler<GetReservaDetallesByReservaQuery, Result<IEnumerable<ReservaDetalleDto>>>
{
    public async Task<Result<IEnumerable<ReservaDetalleDto>>> Handle(
        GetReservaDetallesByReservaQuery r, CancellationToken ct)
    {
        var list = await repo.GetByReservaAsync(r.ICodReserva, ct);
        return Result<IEnumerable<ReservaDetalleDto>>.Success(list.Select(ReservaDetalleDto.FromEntity));
    }
}
