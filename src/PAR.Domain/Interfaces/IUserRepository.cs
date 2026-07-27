using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
    Task<User> CreateAsync(User user, CancellationToken ct = default);
    Task<User> UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<UserPasswordHistory>> GetPasswordHistoryAsync(int userId, int limit, CancellationToken ct = default);
    Task AddPasswordHistoryAsync(UserPasswordHistory history, CancellationToken ct = default);
    Task LockUserAsync(int userId, CancellationToken ct = default);
    Task UnlockUserAsync(int userId, CancellationToken ct = default);
    Task<IEnumerable<int>> GetUserDirectPermissionIdsAsync(int userId, CancellationToken ct = default);
    Task UpdateUserDirectPermissionsAsync(int userId, IEnumerable<int> permissionIds, CancellationToken ct = default);
}
