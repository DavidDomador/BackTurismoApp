using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Transporte.Commands;
using PAR.Application.Features.Transporte.Queries;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class TransporteController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetTransportesQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTransporteByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransporteRequest req, CancellationToken ct)
    {
        var cmd = new CreateTransporteCommand(
            req.ChoferNombre, req.ChoferApellidos, req.ChoferDni, req.ChoferLicencia, req.ChoferTelefono,
            req.VehiculoPlaca, req.VehiculoMarca, req.VehiculoModelo, req.VehiculoAnio, req.VehiculoColor,
            req.VehiculoCantidadAsientos, req.FechaAsignacion ?? DateTime.UtcNow, CurrentUserId);
        var result = await mediator.Send(cmd, ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodigoChoferVehiculo }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransporteRequest req, CancellationToken ct)
    {
        var cmd = new UpdateTransporteCommand(
            id,
            req.ChoferNombre, req.ChoferApellidos, req.ChoferDni, req.ChoferLicencia, req.ChoferTelefono,
            req.VehiculoPlaca, req.VehiculoMarca, req.VehiculoModelo, req.VehiculoAnio, req.VehiculoColor,
            req.VehiculoCantidadAsientos, CurrentUserId);
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteTransporteCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetTransportesQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "Cód.", "Chofer", "DNI Chofer", "Licencia", "Teléfono", "Placa", "Marca", "Modelo", "Año", "Color" };
        var rows = result.Data!.Select(t => new object?[]
        {
            t.ICodigoChoferVehiculo,
            $"{t.Chofer.GNombre} {t.Chofer.GApellidos}",
            t.Chofer.GDni, t.Chofer.GLicencia, t.Chofer.GTelefono,
            t.Vehiculo.GPlaca, t.Vehiculo.GMarca, t.Vehiculo.GModelo, t.Vehiculo.GAnio, t.Vehiculo.GColor
        });

        var bytes = excel.Generate("Transporte", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transporte.xlsx");
    }
}

public record CreateTransporteRequest(
    string ChoferNombre, string ChoferApellidos, string ChoferDni, string ChoferLicencia, string? ChoferTelefono,
    string VehiculoPlaca, string VehiculoMarca, string VehiculoModelo, int? VehiculoAnio, string? VehiculoColor,
    int? VehiculoCantidadAsientos, DateTime? FechaAsignacion);

public record UpdateTransporteRequest(
    string ChoferNombre, string ChoferApellidos, string ChoferDni, string ChoferLicencia, string? ChoferTelefono,
    string VehiculoPlaca, string VehiculoMarca, string VehiculoModelo, int? VehiculoAnio, string? VehiculoColor,
    int? VehiculoCantidadAsientos);
