using Microsoft.EntityFrameworkCore;
using PAR.Domain.Entities;
using PAR.Domain.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Persistence;

public class LoginAttemptRepository(AppDbContext context) : ILoginAttemptRepository
{
    public async Task AddAttemptAsync(LoginAttempt attempt, CancellationToken ct = default)
    {
        context.LoginAttempts.Add(attempt);
        await context.SaveChangesAsync(ct);
    }

    public async Task<int> GetRecentFailedAttemptsAsync(string ipAddress, int windowMinutes, CancellationToken ct = default)
    {
        var since = DateTime.UtcNow.AddMinutes(-windowMinutes);
        return await context.LoginAttempts
            .CountAsync(a => a.IpAddress == ipAddress && !a.Success && a.AttemptedAt >= since, ct);
    }

    public async Task<int> GetRecentFailedAttemptsByUserAsync(string username, int windowMinutes, CancellationToken ct = default)
    {
        var since = DateTime.UtcNow.AddMinutes(-windowMinutes);
        return await context.LoginAttempts
            .CountAsync(a => a.Username == username && !a.Success && a.AttemptedAt >= since, ct);
    }

    public async Task ClearAttemptsAsync(string ipAddress, CancellationToken ct = default)
    {
        await context.LoginAttempts
            .Where(a => a.IpAddress == ipAddress && !a.Success)
            .ExecuteDeleteAsync(ct);
    }
}
