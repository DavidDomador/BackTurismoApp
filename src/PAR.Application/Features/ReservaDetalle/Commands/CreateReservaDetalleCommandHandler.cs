using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.ReservaDetalle.DTOs;
using PAR.Domain.Ports;

namespace PAR.Application.Features.ReservaDetalle.Commands;

public class CreateReservaDetalleCommandHandler(
    IReservaDetalleRepository repo,
    IReservaRepository reservaRepo,
    IClienteRepository clienteRepo)
    : IRequestHandler<CreateReservaDetalleCommand, Result<ReservaDetalleDto>>
{
    public async Task<Result<ReservaDetalleDto>> Handle(CreateReservaDetalleCommand r, CancellationToken ct)
    {
        if (!await reservaRepo.ExistsAsync(r.ICodReserva, ct))
            return Result<ReservaDetalleDto>.Failure("Reserva no encontrada.", 404);

        if (!await clienteRepo.ExistsAsync(r.ICodCliente, ct))
            return Result<ReservaDetalleDto>.Failure("Cliente no encontrado.", 404);

        var entity = new Domain.Entities.ReservaDetalle
        {
            ICodReserva     = r.ICodReserva,
            ICodCliente     = r.ICodCliente,
            EstadoCliente   = r.EstadoCliente,
            CreatedByUserId = r.UserId,
            IsActive        = true
        };

        var created = await repo.CreateAsync(entity, ct);
        return Result<ReservaDetalleDto>.Success(ReservaDetalleDto.FromEntity(created));
    }
}
