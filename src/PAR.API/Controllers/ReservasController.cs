using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Reserva.Commands;
using PAR.Application.Features.Reserva.Queries;
using PAR.Application.Features.ReservaDetalle.Commands;
using PAR.Application.Features.ReservaDetalle.Queries;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ReservasController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetReservasQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetReservaByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateReservaCommand(
            req.ICodPaquete, req.FechaTour, req.RTarifa,
            req.RAbono,
            req.RIncluye, req.RNoIncluye, req.RObservacion,
            req.Estado,
            CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodReserva }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateReservaCommand(
            id, req.ICodPaquete, req.FechaTour, req.RTarifa,
            req.RAbono,
            req.RIncluye, req.RNoIncluye, req.RObservacion,
            req.Estado,
            CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteReservaCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}/pdf")]
    public async Task<IActionResult> GetPdf(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetReservaPdfQuery(id), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return File(result.Data!, MediaTypeNames.Application.Pdf, $"reserva-{id}.pdf");
    }

    // ── ReservaDetalle ────────────────────────────────────────────────

    [HttpGet("{id:int}/detalles")]
    public async Task<IActionResult> GetDetalles(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetReservaDetallesByReservaQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost("{id:int}/detalles")]
    public async Task<IActionResult> AddDetalle(int id, [FromBody] CreateReservaDetalleRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateReservaDetalleCommand(id, req.ICodCliente, req.EstadoCliente, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPut("{id:int}/detalles/{detalleId:int}")]
    public async Task<IActionResult> UpdateDetalle(int id, int detalleId, [FromBody] UpdateReservaDetalleRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateReservaDetalleCommand(detalleId, req.ICodCliente, req.EstadoCliente, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}/detalles/{detalleId:int}")]
    public async Task<IActionResult> DeleteDetalle(int id, int detalleId, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteReservaDetalleCommand(detalleId), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetReservasQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "N° Reserva", "Paquete", "Fecha Tour", "Tarifa", "Pasajeros", "Total", "Abono", "Saldo" };
        var rows = result.Data!.Select(r => new object?[]
        {
            r.RNumeroReserva, r.PaqueteNombre, r.FechaTour,
            r.RTarifa, r.PasajerosCount, r.RTotal, r.RAbono, r.RSaldo
        });

        var bytes = excel.Generate("Reservas", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reservas.xlsx");
    }
}

public record CreateReservaDetalleRequest(int ICodCliente, int? EstadoCliente);
public record UpdateReservaDetalleRequest(int ICodCliente, int? EstadoCliente);

public record CreateReservaRequest(
    int ICodPaquete, DateTime FechaTour, decimal? RTarifa,
    decimal? RAbono,
    string? RIncluye, string? RNoIncluye, string? RObservacion,
    int? Estado);

public record UpdateReservaRequest(
    int ICodPaquete, DateTime FechaTour, decimal? RTarifa,
    decimal? RAbono,
    string? RIncluye, string? RNoIncluye, string? RObservacion,
    int? Estado);
