using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Queries;

public class GetReservasQueryHandler(IReservaRepository repo)
    : IRequestHandler<GetReservasQuery, Result<IEnumerable<ReservaDto>>>
{
    public async Task<Result<IEnumerable<ReservaDto>>> Handle(GetReservasQuery r, CancellationToken ct)
    {
        var list = await repo.GetAllAsync(ct);
        return Result<IEnumerable<ReservaDto>>.Success(list.Select(ReservaDto.FromEntity));
    }
}
