using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Empresa.Commands;
using PAR.Application.Features.Empresa.Queries;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class EmpresasController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmpresasQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    // Devuelve el único registro de empresa (para uso en PDFs y cabeceras)
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmpresasQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        var empresa = result.Data!.FirstOrDefault();
        return empresa is null ? NotFound(new { error = "No hay empresa registrada." }) : Ok(empresa);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmpresaByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmpresaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateEmpresaCommand(
            req.ERazonSocial, req.ERUC,
            req.EDireccion, req.ETelefono, req.ECorreo,
            CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodEmpresa }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmpresaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateEmpresaCommand(
            id, req.ERazonSocial, req.ERUC,
            req.EDireccion, req.ETelefono, req.ECorreo,
            CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteEmpresaCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmpresasQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "Cód.", "Razón Social", "RUC", "Dirección", "Teléfono", "Correo" };
        var rows = result.Data!.Select(e => new object?[]
        {
            e.ICodEmpresa, e.ERazonSocial, e.ERUC, e.EDireccion, e.ETelefono, e.ECorreo
        });

        var bytes = excel.Generate("Empresa", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "empresa.xlsx");
    }
}

public record CreateEmpresaRequest(
    string ERazonSocial, string? ERUC,
    string? EDireccion, string? ETelefono, string? ECorreo);

public record UpdateEmpresaRequest(
    string ERazonSocial, string? ERUC,
    string? EDireccion, string? ETelefono, string? ECorreo);
