using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface ILoginAttemptRepository
{
    Task AddAttemptAsync(LoginAttempt attempt, CancellationToken ct = default);
    Task<int> GetRecentFailedAttemptsAsync(string ipAddress, int windowMinutes, CancellationToken ct = default);
    Task<int> GetRecentFailedAttemptsByUserAsync(string username, int windowMinutes, CancellationToken ct = default);
    Task ClearAttemptsAsync(string ipAddress, CancellationToken ct = default);
}
