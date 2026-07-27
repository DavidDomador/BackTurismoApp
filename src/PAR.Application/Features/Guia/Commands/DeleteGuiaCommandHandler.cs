using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Guia.Commands;

public class DeleteGuiaCommandHandler(IGuiaRepository repo) : IRequestHandler<DeleteGuiaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteGuiaCommand request, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(request.Id, ct))
            return Result<bool>.Failure("Guia not found.", 404);
        await repo.DeleteAsync(request.Id, ct);
        return Result<bool>.Success(true);
    }
}
