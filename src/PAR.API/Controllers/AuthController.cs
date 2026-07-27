using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PAR.Application.Features.Auth.Commands;

namespace PAR.API.Controllers;

/// <summary>
/// Autenticación de usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Iniciar sesión con usuario y contraseña
    /// </summary>
    /// <param name="request">Credenciales de acceso</param>
    /// <returns>Token JWT e información del usuario</returns>
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var command = new LoginCommand(request.Username, request.Password, ipAddress);
        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }
}

public record LoginRequest(string Username, string Password);
