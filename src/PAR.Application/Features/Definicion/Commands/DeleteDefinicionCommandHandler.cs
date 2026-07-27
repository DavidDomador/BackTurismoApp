using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class DeleteDefinicionCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<DeleteDefinicionCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteDefinicionCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Definición no encontrada.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
