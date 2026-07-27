using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAR.Application.Features.Users.Commands;
using PAR.Application.Features.Users.Queries;
using PAR.Application.Features.Roles.Queries;
using PAR.Application.Features.Users.DTOs;
using PAR.Shared.Excel;

namespace PAR.API.Controllers;

/// <summary>
/// User management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UsersController(IMediator mediator, IExcelExportService excel) : ControllerBase
{
    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetUsersQuery(), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(request.Username, request.Email, request.Password, request.FirstName, request.LastName);
        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Change user password (last 3 passwords are restricted)
    /// </summary>
    [HttpPut("{id:int}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request, CancellationToken ct)
    {
        var command = new ChangePasswordCommand(id, request.CurrentPassword, request.NewPassword);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { message = "Password changed successfully." }) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Lock a user account
    /// </summary>
    [HttpPut("{id:int}/lock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Lock(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new LockUserCommand(id), ct);
        return result.IsSuccess ? Ok(new { message = "User locked." }) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Unlock a user account
    /// </summary>
    [HttpPut("{id:int}/unlock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unlock(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new UnlockUserCommand(id), ct);
        return result.IsSuccess ? Ok(new { message = "User unlocked." }) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Update user profile (email, name, active status)
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var command = new UpdateUserCommand(id, request.Email, request.FirstName, request.LastName, request.IsActive);
        var result  = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteUserCommand(id), ct);
        return result.IsSuccess ? NoContent() : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Get role IDs assigned to a user
    /// </summary>
    [HttpGet("{id:int}/roles")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserRolesQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Replace the roles assigned to a user
    /// </summary>
    [HttpPut("{id:int}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoles(int id, [FromBody] UpdateUserRolesRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateUserRolesCommand(id, request.RoleIds), ct);
        return result.IsSuccess ? Ok(new { message = "Roles updated." }) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Get combined permission summary for a user (role-inherited + direct)
    /// </summary>
    [HttpGet("{id:int}/permission-summary")]
    [ProducesResponseType(typeof(UserPermissionSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPermissionSummary(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserPermissionSummaryQuery(id), ct);
        return result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Replace direct (non-role) permissions assigned to a user
    /// </summary>
    [HttpPut("{id:int}/direct-permissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDirectPermissions(int id, [FromBody] UpdateDirectPermissionsRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateUserDirectPermissionsCommand(id, request.PermissionIds), ct);
        return result.IsSuccess ? Ok(new { message = "Direct permissions updated." }) : StatusCode(result.StatusCode, new { error = result.Error });
    }

    [HttpGet("excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken ct)
    {
        var result = await mediator.Send(new GetUsersQuery(), ct);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, new { error = result.Error });

        var headers = new[] { "ID", "Usuario", "Email", "Nombre", "Apellido", "Activo" };
        var rows = result.Data!.Select(u => new object?[]
        {
            u.Id, u.Username, u.Email, u.FirstName, u.LastName, u.IsActive
        });

        var bytes = excel.Generate("Usuarios", headers, rows);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "usuarios.xlsx");
    }
}

public record CreateUserRequest(string Username, string Email, string Password, string? FirstName, string? LastName);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
public record UpdateUserRequest(string Email, string? FirstName, string? LastName, bool IsActive);
public record UpdateUserRolesRequest(IList<int> RoleIds);
public record UpdateDirectPermissionsRequest(IList<int> PermissionIds);
