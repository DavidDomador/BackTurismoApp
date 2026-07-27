using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Definicion.Commands;
using PAR.Application.Features.Definicion.Queries;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DefinicionesController(IMediator mediator) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    // ── Definicion ────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetDefinicionesQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDefinicionByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDefinicionRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateDefinicionCommand(req.DNombre, req.DDescripcion, req.Estado, CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdDef }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDefinicionRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateDefinicionCommand(id, req.DNombre, req.DDescripcion, req.Estado, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteDefinicionCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    // ── DefinicionDetalle ──────────────────────────────────────────────

    [HttpGet("{idDef:int}/detalles")]
    public async Task<IActionResult> GetDetalles(int idDef, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDefinicionByIdQuery(idDef), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return Ok(result.Data!.Detalles.Where(d => d.IsActive && d.Estado));
    }

    [HttpPost("{idDef:int}/detalles")]
    public async Task<IActionResult> CreateDetalle(int idDef, [FromBody] CreateDetalleRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateDefinicionDetalleCommand(idDef, req.DdValue, req.DdAbreviacion, req.DdDescripcion, req.Estado, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPut("{idDef:int}/detalles/{id:int}")]
    public async Task<IActionResult> UpdateDetalle(int idDef, int id, [FromBody] UpdateDetalleRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateDefinicionDetalleCommand(id, req.DdValue, req.DdAbreviacion, req.DdDescripcion, req.Estado, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{idDef:int}/detalles/{id:int}")]
    public async Task<IActionResult> DeleteDetalle(int idDef, int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteDefinicionDetalleCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }
}

public record CreateDefinicionRequest(string DNombre, string? DDescripcion, bool Estado);
public record UpdateDefinicionRequest(string DNombre, string? DDescripcion, bool Estado);
public record CreateDetalleRequest(string DdValue, string? DdAbreviacion, string? DdDescripcion, bool Estado);
public record UpdateDetalleRequest(string DdValue, string? DdAbreviacion, string? DdDescripcion, bool Estado);
