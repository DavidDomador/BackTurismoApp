using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Reserva.DTOs;

namespace PAR.Application.Features.Reserva.Commands;

public record UpdateReservaCommand(
    int Id, int ICodPaquete, DateTime FechaTour, decimal? RTarifa,
    decimal? RAbono,
    string? RIncluye, string? RNoIncluye, string? RObservacion,
    int? Estado,
    int? UserId)
    : IRequest<Result<ReservaDto>>;
