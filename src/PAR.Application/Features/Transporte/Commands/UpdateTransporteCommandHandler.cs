using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Transporte.Commands;

public class UpdateTransporteCommandHandler(ITransporteRepository repo) : IRequestHandler<UpdateTransporteCommand, Result<TransporteDto>>
{
    public async Task<Result<TransporteDto>> Handle(UpdateTransporteCommand r, CancellationToken ct)
    {
        if (!await repo.ExistsAsync(r.Id, ct))
            return Result<TransporteDto>.Failure("Transporte not found.", 404);

        var choferData   = new Chofer   { GNombre = r.ChoferNombre,   GApellidos = r.ChoferApellidos,   GDni = r.ChoferDni,   GLicencia = r.ChoferLicencia,   GTelefono = r.ChoferTelefono };
        var vehiculoData = new Vehiculo { GPlaca = r.VehiculoPlaca, GMarca = r.VehiculoMarca, GModelo = r.VehiculoModelo, GAnio = r.VehiculoAnio, GColor = r.VehiculoColor, GCantidadAsientos = r.VehiculoCantidadAsientos };

        var cv = await repo.UpdateAsync(r.Id, choferData, vehiculoData, r.UserId, ct);
        return Result<TransporteDto>.Success(TransporteDto.FromEntity(cv));
    }
}
