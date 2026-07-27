using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Itinerario.Commands;
using PAR.Application.Features.Itinerario.Queries;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ItinerariosController(IMediator mediator, IWebHostEnvironment env) : ControllerBase
{
    private int? CurrentUserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;
    private string UploadsPath => Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "itinerarios");

    // ── Consulta por paquete ──────────────────────────────────────────

    [HttpGet("paquete/{iCodPaquete:int}")]
    public async Task<IActionResult> GetByPaquete(int iCodPaquete, CancellationToken ct)
    {
        var result = await mediator.Send(new GetItinerariosByPaqueteQuery(iCodPaquete), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    // ── Subir imagen ──────────────────────────────────────────────────

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImagen(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No se proporcionó ningún archivo." });

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            return BadRequest(new { error = "Formato de imagen no permitido." });

        Directory.CreateDirectory(UploadsPath);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(UploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { fileName });
    }

    // ── Obtener imagen ────────────────────────────────────────────────

    [HttpGet("imagen/{fileName}")]
    [AllowAnonymous]
    public IActionResult GetImagen(string fileName)
    {
        var filePath = Path.Combine(UploadsPath, Path.GetFileName(fileName));
        if (!System.IO.File.Exists(filePath)) return NotFound();
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var mimeType = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png"  => "image/png",
            ".webp" => "image/webp",
            ".gif"  => "image/gif",
            _       => "application/octet-stream"
        };
        return PhysicalFile(filePath, mimeType);
    }

    // ── CRUD ──────────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItinerarioRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateItinerarioCommand(
            req.ICodPaquete, req.INombre, req.IDescripcion, req.Icon, req.Imagen, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItinerarioRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateItinerarioCommand(
            id, req.INombre, req.IDescripcion, req.Icon, req.Imagen, CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteItinerarioCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }
}

public record CreateItinerarioRequest(
    int ICodPaquete, string INombre, string? IDescripcion, string? Icon, string? Imagen);

public record UpdateItinerarioRequest(
    string INombre, string? IDescripcion, string? Icon, string? Imagen);
