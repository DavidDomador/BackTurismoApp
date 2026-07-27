using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Salida.Commands;
using PAR.Application.Features.Salida.Queries;
using PAR.Application.Ports;

namespace PAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SalidasController(IMediator mediator, ISalidaExcelService salidaExcel) : ControllerBase
{
    private int? CurrentUserId =>
        int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetSalidasQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSalidaByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalidaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateSalidaCommand(
            req.ReservaIds,
            req.ICodigoGuia,
            req.ICodChoferVehiculo,
            req.FechaSalida,
            req.HoraSalida,
            CurrentUserId), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ICodSalida }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSalidaRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateSalidaCommand(
            id,
            req.ReservaIds,
            req.ICodigoGuia,
            req.ICodChoferVehiculo,
            req.FechaSalida,
            req.HoraSalida,
            CurrentUserId), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteSalidaCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var bytes = await salidaExcel.GenerarAsync(ct);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "salidas-pasajeros.xlsx");
    }
}

public record CreateSalidaRequest(
    IEnumerable<int> ReservaIds,
    int ICodigoGuia,
    int ICodChoferVehiculo,
    DateTime FechaSalida,
    TimeSpan HoraSalida);

public record UpdateSalidaRequest(
    IEnumerable<int> ReservaIds,
    int ICodigoGuia,
    int ICodChoferVehiculo,
    DateTime FechaSalida,
    TimeSpan HoraSalida);
