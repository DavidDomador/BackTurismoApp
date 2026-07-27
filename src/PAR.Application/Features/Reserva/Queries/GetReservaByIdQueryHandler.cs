using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Queries;

public class GetReservaByIdQueryHandler(IReservaRepository repo)
    : IRequestHandler<GetReservaByIdQuery, Result<ReservaDto>>
{
    public async Task<Result<ReservaDto>> Handle(GetReservaByIdQuery r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        return entity is null
            ? Result<ReservaDto>.Failure("Reserva no encontrada.", 404)
            : Result<ReservaDto>.Success(ReservaDto.FromEntity(entity));
    }
}
