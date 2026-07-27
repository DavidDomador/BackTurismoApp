using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Transporte.Commands;

public class CreateTransporteCommandHandler(ITransporteRepository repo) : IRequestHandler<CreateTransporteCommand, Result<TransporteDto>>
{
    public async Task<Result<TransporteDto>> Handle(CreateTransporteCommand r, CancellationToken ct)
    {
        var chofer = new Chofer
        {
            GNombre    = r.ChoferNombre,
            GApellidos = r.ChoferApellidos,
            GDni       = r.ChoferDni,
            GLicencia  = r.ChoferLicencia,
            GTelefono  = r.ChoferTelefono,
            IsActive   = true
        };
        var vehiculo = new Vehiculo
        {
            GPlaca              = r.VehiculoPlaca,
            GMarca              = r.VehiculoMarca,
            GModelo             = r.VehiculoModelo,
            GAnio               = r.VehiculoAnio,
            GColor              = r.VehiculoColor,
            GCantidadAsientos   = r.VehiculoCantidadAsientos,
            IsActive            = true
        };
        var cv = await repo.CreateAsync(chofer, vehiculo, r.FechaAsignacion, r.UserId, ct);
        return Result<TransporteDto>.Success(TransporteDto.FromEntity(cv));
    }
}
