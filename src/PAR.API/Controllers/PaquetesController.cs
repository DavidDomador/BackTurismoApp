using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Paquete.Commands;
using PAR.Application.Features.Paquete.Queries;
using PAR.Shared.Excel;
using System.Net.Mime;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class PaquetesController(IMediator mediator, IExcelExportService excel, IWebHostEnvironment env) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;
    private string PdfAdjuntosPath => Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "paquetes-pdf");

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetPaquetesQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPaqueteByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaqueteRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreatePaqueteCommand(
            req.PNombre, req.PDescripcion, req.PPrecioBase,
            req.PPrecioDescuento, req.PPrecioReserva, CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodPaquete }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaqueteRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdatePaqueteCommand(
            id, req.PNombre, req.PDescripcion, req.PPrecioBase,
            req.PPrecioDescuento, req.PPrecioReserva, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeletePaqueteCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}/pdf")]
    public async Task<IActionResult> GetPdf(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPaquetePdfQuery(id), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return File(result.Data!, MediaTypeNames.Application.Pdf, $"paquete-{id}.pdf");
    }

    // ── PDF Adjunto ──────────────────────────────────────────────────────────

    /// <summary>Sube un PDF adjunto al paquete (reemplaza el anterior si existe).</summary>
    [HttpPost("{id:int}/pdf-adjunto")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPdfAdjunto(int id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No se proporcionó ningún archivo." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext != ".pdf")
            return BadRequest(new { error = "Solo se permiten archivos PDF." });

        // Obtener el paquete actual para eliminar el PDF anterior si existe
        var currentResult = await mediator.Send(new GetPaqueteByIdQuery(id), ct);
        if (!currentResult.IsSuccess)
            return StatusCode(currentResult.StatusCode, new { error = currentResult.Error });

        // Eliminar archivo anterior del disco
        if (!string.IsNullOrEmpty(currentResult.Data!.PdfAdjunto))
        {
            var oldPath = Path.Combine(PdfAdjuntosPath, currentResult.Data.PdfAdjunto);
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
        }

        // Guardar nuevo archivo
        Directory.CreateDirectory(PdfAdjuntosPath);
        var fileName = $"{Guid.NewGuid()}.pdf";
        var filePath = Path.Combine(PdfAdjuntosPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        // Actualizar BD
        var result = await mediator.Send(new UpdatePaquetePdfAdjuntoCommand(id, fileName), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>Elimina el PDF adjunto del paquete.</summary>
    [HttpDelete("{id:int}/pdf-adjunto")]
    public async Task<IActionResult> DeletePdfAdjunto(int id, CancellationToken ct)
    {
        var currentResult = await mediator.Send(new GetPaqueteByIdQuery(id), ct);
        if (!currentResult.IsSuccess)
            return StatusCode(currentResult.StatusCode, new { error = currentResult.Error });

        if (string.IsNullOrEmpty(currentResult.Data!.PdfAdjunto))
            return BadRequest(new { error = "Este paquete no tiene PDF adjunto." });

        // Eliminar archivo del disco
        var filePath = Path.Combine(PdfAdjuntosPath, currentResult.Data.PdfAdjunto);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        // Limpiar BD
        var result = await mediator.Send(new UpdatePaquetePdfAdjuntoCommand(id, null), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    // ── Excel ────────────────────────────────────────────────────────────────

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetPaquetesQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "Cód.", "Nombre", "Descripción", "Precio Base", "Precio Descuento", "Precio Reserva" };
        var rows = result.Data!.Select(p => new object?[]
        {
            p.ICodPaquete, p.PNombre, p.PDescripcion,
            p.PPrecioBase, p.PPrecioDescuento, p.PPrecioReserva
        });

        var bytes = excel.Generate("Paquetes", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "paquetes.xlsx");
    }
}

public record CreatePaqueteRequest(
    string PNombre, string? PDescripcion,
    decimal PPrecioBase, decimal? PPrecioDescuento, decimal? PPrecioReserva);

public record UpdatePaqueteRequest(
    string PNombre, string? PDescripcion,
    decimal PPrecioBase, decimal? PPrecioDescuento, decimal? PPrecioReserva);
