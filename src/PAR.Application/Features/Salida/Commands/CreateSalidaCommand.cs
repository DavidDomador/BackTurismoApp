using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;

namespace PAR.Application.Features.Salida.Commands;

public record CreateSalidaCommand(
    IEnumerable<int> ReservaIds,
    int ICodigoGuia,
    int ICodChoferVehiculo,
    DateTime FechaSalida,
    TimeSpan HoraSalida,
    int? UserId)
    : IRequest<Result<SalidaDto>>;
