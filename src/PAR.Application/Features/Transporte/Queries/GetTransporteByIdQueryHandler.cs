using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Transporte.Queries;

public class GetTransporteByIdQueryHandler(ITransporteRepository repo) : IRequestHandler<GetTransporteByIdQuery, Result<TransporteDto>>
{
    public async Task<Result<TransporteDto>> Handle(GetTransporteByIdQuery request, CancellationToken ct)
    {
        var cv = await repo.GetByIdAsync(request.Id, ct);
        if (cv is null) return Result<TransporteDto>.Failure("Transporte not found.", 404);
        return Result<TransporteDto>.Success(TransporteDto.FromEntity(cv));
    }
}
