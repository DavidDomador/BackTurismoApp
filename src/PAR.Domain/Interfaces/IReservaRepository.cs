using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IReservaRepository
{
    Task<IEnumerable<Reserva>> GetAllAsync(CancellationToken ct = default);
    Task<Reserva?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Reserva> CreateAsync(Reserva reserva, CancellationToken ct = default);
    Task<Reserva> UpdateAsync(Reserva reserva, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<int> GetNextNumeroReservaAsync(CancellationToken ct = default);
}
