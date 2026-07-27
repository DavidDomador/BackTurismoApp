using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Roles.Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission).FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default) =>
        await context.Roles.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken ct = default) =>
        await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync(ct);

    public async Task<IEnumerable<int>> GetUserRoleIdsAsync(int userId, CancellationToken ct = default) =>
        await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(ct);

    public async Task UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds, CancellationToken ct = default)
    {
        await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .ExecuteDeleteAsync(ct);

        var newRoles = roleIds.Select(rId => new UserRole { UserId = userId, RoleId = rId });
        context.UserRoles.AddRange(newRoles);
        await context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken ct = default) =>
        await context.Permissions.AsNoTracking().OrderBy(p => p.Module).ThenBy(p => p.Name).ToListAsync(ct);

    public async Task<IEnumerable<int>> GetUserRolePermissionIdsAsync(int userId, CancellationToken ct = default) =>
        await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.PermissionId)
            .Distinct()
            .ToListAsync(ct);
}
