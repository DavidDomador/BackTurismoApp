using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Guia.Commands;
using PAR.Application.Features.Guia.Queries;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GuiasController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetGuiasQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetGuiaByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuiaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateGuiaCommand(req.GNombre, req.GApellidos, req.GDni, req.GCorreo, CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodigoGuia }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGuiaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateGuiaCommand(id, req.GNombre, req.GApellidos, req.GDni, req.GCorreo, req.IsActive, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteGuiaCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetGuiasQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "Cód.", "Nombre", "Apellidos", "DNI", "Correo" };
        var rows = result.Data!.Select(g => new object?[]
        {
            g.ICodigoGuia, g.GNombre, g.GApellidos, g.GDni, g.GCorreo
        });

        var bytes = excel.Generate("Guías", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "guias.xlsx");
    }
}

public record CreateGuiaRequest(string GNombre, string GApellidos, string GDni, string GCorreo);
public record UpdateGuiaRequest(string GNombre, string GApellidos, string GDni, string GCorreo, bool IsActive);
