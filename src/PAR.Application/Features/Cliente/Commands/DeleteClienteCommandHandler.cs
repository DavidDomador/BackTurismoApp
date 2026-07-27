using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Cliente.Commands;

public class DeleteClienteCommandHandler(IClienteRepository repo)
    : IRequestHandler<DeleteClienteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteClienteCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Cliente no encontrado.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
