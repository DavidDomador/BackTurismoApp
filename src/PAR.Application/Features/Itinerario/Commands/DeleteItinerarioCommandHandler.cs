using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Itinerario.Commands;

public class DeleteItinerarioCommandHandler(IItinerarioRepository repo)
    : IRequestHandler<DeleteItinerarioCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteItinerarioCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<bool>.Failure("Itinerario no encontrado.", 404);

        await repo.DeleteAsync(r.Id, ct);
        return Result<bool>.Success(true);
    }
}
