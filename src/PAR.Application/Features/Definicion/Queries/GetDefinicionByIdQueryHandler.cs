using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Queries;

public class GetDefinicionByIdQueryHandler(IDefinicionRepository repo)
    : IRequestHandler<GetDefinicionByIdQuery, Result<DefinicionDto>>
{
    public async Task<Result<DefinicionDto>> Handle(GetDefinicionByIdQuery request, CancellationToken ct)
    {
        var def = await repo.GetByIdAsync(request.Id, ct);
        if (def is null) return Result<DefinicionDto>.Failure("Definición no encontrada.", 404);
        return Result<DefinicionDto>.Success(DefinicionDto.FromEntity(def));
    }
}
