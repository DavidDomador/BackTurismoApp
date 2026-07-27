using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public class DeleteReservaDetalleCommandHandler(IReservaDetalleRepository repo)
    : IRequestHandler<DeleteReservaDetalleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteReservaDetalleCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Detalle no encontrado.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
