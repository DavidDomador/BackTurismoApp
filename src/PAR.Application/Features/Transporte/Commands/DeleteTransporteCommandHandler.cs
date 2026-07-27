using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Transporte.Commands;

public class DeleteTransporteCommandHandler(ITransporteRepository repo) : IRequestHandler<DeleteTransporteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTransporteCommand request, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(request.Id, ct))
            return Result<bool>.Failure("Transporte not found.", 404);
        await repo.DeleteAsync(request.Id, ct);
        return Result<bool>.Success(true);
    }
}
