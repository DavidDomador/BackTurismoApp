using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Definicion.Commands;

public class CreateDefinicionCommandHandler(IDefinicionRepository repo)
    : IRequestHandler<CreateDefinicionCommand, Result<DefinicionDto>>
{
    public async Task<Result<DefinicionDto>> Handle(CreateDefinicionCommand r, CancellationToken ct)
    {
        var entity = new Domain.Entities.Definicion
        {
            DNombre      = r.DNombre,
            DDescripcion = r.DDescripcion,
            Estado       = r.Estado,
            CreatedByUserId = r.UserId,
            IsActive     = true
        };
        var created = await repo.CreateAsync(entity, ct);
        return Result<DefinicionDto>.Success(DefinicionDto.FromEntity(created));
    }
}
