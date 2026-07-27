using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Reserva.Commands;

public class DeleteReservaCommandHandler(IReservaRepository repo)
    : IRequestHandler<DeleteReservaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteReservaCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Reserva no encontrada.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
