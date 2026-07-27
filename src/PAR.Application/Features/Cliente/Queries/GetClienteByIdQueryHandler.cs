using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Cliente.Queries;

public class GetClienteByIdQueryHandler(IClienteRepository repo)
    : IRequestHandler<GetClienteByIdQuery, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> Handle(GetClienteByIdQuery r, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(r.Id, ct);
        return entity is null
            ? Result<ClienteDto>.Failure("Cliente no encontrado.", 404)
            : Result<ClienteDto>.Success(ClienteDto.FromEntity(entity));
    }
}
