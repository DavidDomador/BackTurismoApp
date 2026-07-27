using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Salida.Queries;

public class GetSalidaByIdQueryHandler(ISalidaRepository repo)
    : IRequestHandler<GetSalidaByIdQuery, Result<SalidaDto>>
{
    public async Task<Result<SalidaDto>> Handle(GetSalidaByIdQuery request, CancellationToken ct)
    {
        var salida = await repo.GetByIdAsync(request.Id, ct);
        if (salida is null)
            return Result<SalidaDto>.Failure("Salida no encontrada.", 404);
        return Result<SalidaDto>.Success(SalidaDto.FromEntity(salida));
    }
}
