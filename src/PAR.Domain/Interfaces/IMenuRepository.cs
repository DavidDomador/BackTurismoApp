using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IMenuRepository
{
    Task<IEnumerable<Menu>> GetAllActiveAsync(CancellationToken ct = default);
}
