using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;

namespace PAR.Application.Features.Transporte.Commands;

public record UpdateTransporteCommand(
    int Id,
    string ChoferNombre,
    string ChoferApellidos,
    string ChoferDni,
    string ChoferLicencia,
    string? ChoferTelefono,
    string VehiculoPlaca,
    string VehiculoMarca,
    string VehiculoModelo,
    int? VehiculoAnio,
    string? VehiculoColor,
    int? VehiculoCantidadAsientos,
    int? UserId
) : IRequest<Result<TransporteDto>>;
