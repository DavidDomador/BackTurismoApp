using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken ct = default);
    Task<IEnumerable<int>> GetUserRoleIdsAsync(int userId, CancellationToken ct = default);
    Task UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds, CancellationToken ct = default);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken ct = default);
    Task<IEnumerable<int>> GetUserRolePermissionIdsAsync(int userId, CancellationToken ct = default);
}
