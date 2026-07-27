using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Cliente.Commands;
using PAR.Application.Features.Cliente.Queries;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ClientesController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientesQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetClienteByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClienteRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateClienteCommand(
            req.CNombres, req.CApellidos, req.CDni,
            req.CCorreo, req.CEdad, req.CDireccion, CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodCliente }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateClienteCommand(
            id, req.CNombres, req.CApellidos, req.CDni,
            req.CCorreo, req.CEdad, req.CDireccion, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteClienteCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientesQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "Cód.", "Nombres", "Apellidos", "DNI", "Correo", "Edad", "Dirección" };
        var rows = result.Data!.Select(c => new object?[]
        {
            c.ICodCliente, c.CNombres, c.CApellidos, c.CDni, c.CCorreo, c.CEdad, c.CDireccion
        });

        var bytes = excel.Generate("Clientes", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "clientes.xlsx");
    }
}

public record CreateClienteRequest(
    string CNombres, string CApellidos, string CDni,
    string? CCorreo, int? CEdad, string? CDireccion);

public record UpdateClienteRequest(
    string CNombres, string CApellidos, string CDni,
    string? CCorreo, int? CEdad, string? CDireccion);
