using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Commands;

public class DeletePaqueteCommandHandler(IPaqueteRepository repo)
    : IRequestHandler<DeletePaqueteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePaqueteCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Paquete no encontrado.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
