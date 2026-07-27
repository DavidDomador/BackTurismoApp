using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Salida.Commands;

public class DeleteSalidaCommandHandler(ISalidaRepository repo)
    : IRequestHandler<DeleteSalidaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteSalidaCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.ICodSalida, ct))
            return Result<bool>.Failure("Salida no encontrada.", 404);

        await repo.DeleteAsync(r.ICodSalida, ct);
        return Result<bool>.Success(true);
    }
}
