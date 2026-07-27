using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Venta.Commands;
using PAR.Application.Features.Venta.Queries;
using PAR.Application.Ports;
using PAR.Domain.Ports;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class VentasController(IMediator mediator, IVentaRepository ventaRepo, IExcelExportService excel, IVentaPdfService ventaPdf) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;
    private string CurrentUserName => User.FindFirstValue(ClaimTypes.Name)
        ?? User.FindFirstValue("name")
        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? "Sistema";

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetVentasQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetVentaByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("encargado/{iCodReserva:int}")]
    public async Task<IActionResult> GetEncargadoCliente(int iCodReserva, CancellationToken ct)
    {
        var clienteId = await ventaRepo.GetEncargadoGrupoClienteAsync(iCodReserva, ct);
        return Ok(new { iCodCliente = clienteId });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVentaRequest req, CancellationToken ct)
    {
        var userId = CurrentUserId ?? 0;
        var result = await mediator.Send(new CreateVentaCommand(
            req.ICodReserva,
            req.VFechaVenta,
            userId,
            CurrentUserName,
            req.ICodCliente,
            req.VSaldo,
            CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodVenta }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVentaRequest req, CancellationToken ct)
    {
        var userId = CurrentUserId ?? 0;
        var result = await mediator.Send(new UpdateVentaCommand(
            id,
            req.ICodReserva,
            req.VFechaVenta,
            userId,
            CurrentUserName,
            req.ICodCliente,
            req.VSaldo,
            CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteVentaCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}/pdf")]
    public async Task<IActionResult> DownloadPdf(int id, CancellationToken ct)
    {
        try
        {
            var bytes = await ventaPdf.GenerarAsync(id, ct);
            return File(bytes, "application/pdf", $"boleta-{id}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetVentasQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "N° Venta", "Fecha", "Usuario", "Reserva / Paquete", "Cliente" };
        var rows = result.Data!.Select(v => new object?[]
        {
            v.VNumeroVenta, v.VFechaVenta, v.VUsuarioNombre, v.NombrePaquete, v.NombreCliente
        });

        var bytes = excel.Generate("Ventas", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ventas.xlsx");
    }
}

public record CreateVentaRequest(int ICodReserva, DateTime VFechaVenta, int? ICodCliente, decimal? VSaldo);
public record UpdateVentaRequest(int ICodReserva, DateTime VFechaVenta, int? ICodCliente, decimal? VSaldo);
