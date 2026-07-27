using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Paquete.Queries;

public class GetPaqueteByIdQueryHandler(IPaqueteRepository repo)
    : IRequestHandler<GetPaqueteByIdQuery, Result<PaqueteDto>>
{
    public async Task<Result<PaqueteDto>> Handle(GetPaqueteByIdQuery r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        if (entity is null) return Result<PaqueteDto>.Failure("Paquete no encontrado.", 404);
        return Result<PaqueteDto>.Success(PaqueteDto.FromEntity(entity));
    }
}
