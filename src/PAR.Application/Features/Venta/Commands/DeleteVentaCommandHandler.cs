using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Venta.Commands;

public class DeleteVentaCommandHandler(IVentaRepository repo)
    : IRequestHandler<DeleteVentaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteVentaCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Venta no encontrada.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
