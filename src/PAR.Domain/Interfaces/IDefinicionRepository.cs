using PAR.Domain.Entities;

namespace PAR.Domain.Ports;

public interface IDefinicionRepository
{
    Task<IEnumerable<Definicion>> GetAllAsync(CancellationToken ct = default);
    Task<Definicion?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Definicion> CreateAsync(Definicion definicion, CancellationToken ct = default);
    Task<Definicion> UpdateAsync(Definicion definicion, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);

    // DefinicionDetalle
    Task<IEnumerable<DefinicionDetalle>> GetDetallesByDefIdAsync(int idDef, CancellationToken ct = default);
    Task<DefinicionDetalle?> GetDetalleByIdAsync(int id, CancellationToken ct = default);
    Task<DefinicionDetalle> CreateDetalleAsync(DefinicionDetalle detalle, CancellationToken ct = default);
    Task<DefinicionDetalle> UpdateDetalleAsync(DefinicionDetalle detalle, CancellationToken ct = default);
    Task DeleteDetalleAsync(int id, CancellationToken ct = default);
    Task<bool> DetalleExistsAsync(int id, CancellationToken ct = default);
}
