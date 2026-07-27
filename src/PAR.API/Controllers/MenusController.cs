using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Menus.Queries;

namespace PAR.API.Controllers;

/// <summary>
/// Menús dinámicos del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class MenusController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Devuelve los menús activos filtrados por los permisos del usuario autenticado.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var userPermissions = User.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        var result = await mediator.Send(new GetMenusQuery(userPermissions), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }
}
