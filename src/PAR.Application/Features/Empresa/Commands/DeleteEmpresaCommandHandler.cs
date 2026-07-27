using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Empresa.Commands;

public class DeleteEmpresaCommandHandler(IEmpresaRepository repo)
    : IRequestHandler<DeleteEmpresaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmpresaCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Empresa no encontrada.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
