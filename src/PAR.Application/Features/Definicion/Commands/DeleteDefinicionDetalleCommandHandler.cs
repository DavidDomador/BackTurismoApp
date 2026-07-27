using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class DeleteDefinicionDetalleCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<DeleteDefinicionDetalleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteDefinicionDetalleCommand r, CancellationToken ct)
    {
        if (!await repo.DetalleExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Detalle no encontrado.", 404);

        await repo.DeleteDetalleAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
