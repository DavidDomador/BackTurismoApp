using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) =>
        await context.Users.AsNoTracking().ToListAsync(ct);

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await context.Users.FindAsync([id], ct);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        await context.Users.AnyAsync(u => u.Id == id, ct);

    public async Task<IEnumerable<UserPasswordHistory>> GetPasswordHistoryAsync(int userId, int limit, CancellationToken ct = default) =>
        await context.UserPasswordHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.CreatedAt)
            .Take(limit)
            .ToListAsync(ct);

    public async Task AddPasswordHistoryAsync(UserPasswordHistory history, CancellationToken ct = default)
    {
        context.UserPasswordHistories.Add(history);
        await context.SaveChangesAsync(ct);
    }

    public async Task LockUserAsync(int userId, CancellationToken ct = default)
    {
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.IsLocked, true).SetProperty(u => u.UpdatedAt, DateTime.UtcNow), ct);
    }

    public async Task UnlockUserAsync(int userId, CancellationToken ct = default)
    {
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.IsLocked, false).SetProperty(u => u.UpdatedAt, DateTime.UtcNow), ct);
    }

    public async Task<IEnumerable<int>> GetUserDirectPermissionIdsAsync(int userId, CancellationToken ct = default) =>
        await context.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => up.PermissionId)
            .ToListAsync(ct);

    public async Task UpdateUserDirectPermissionsAsync(int userId, IEnumerable<int> permissionIds, CancellationToken ct = default)
    {
        await context.UserPermissions.Where(up => up.UserId == userId).ExecuteDeleteAsync(ct);
        context.UserPermissions.AddRange(permissionIds.Select(pId => new UserPermission { UserId = userId, PermissionId = pId }));
        await context.SaveChangesAsync(ct);
    }
}
