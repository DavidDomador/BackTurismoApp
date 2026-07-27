namespace PAR.Application.Features.Venta.DTOs;

public class VentaDto
{
    public int ICodVenta { get; set; }
    public string VNumeroVenta { get; set; } = string.Empty;
    public int ICodReserva { get; set; }
    public string? NombrePaquete { get; set; }
    public DateTime VFechaVenta { get; set; }
    public int ICodUsuario { get; set; }
    public string VUsuarioNombre { get; set; } = string.Empty;
    public int? ICodCliente { get; set; }
    public string? NombreCliente { get; set; }
    public decimal VTotal { get; set; }
    public decimal? RSaldo { get; set; }
    public decimal? VSaldo { get; set; }
    public List<VentaDetalleDto> Detalles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static VentaDto FromEntity(Domain.Entities.Venta e) => new()
    {
        ICodVenta      = e.ICodVenta,
        VNumeroVenta   = e.VNumeroVenta,
        ICodReserva    = e.ICodReserva,
        NombrePaquete  = e.Reserva?.Paquete?.PNombre,
        VFechaVenta    = e.VFechaVenta,
        ICodUsuario    = e.ICodUsuario,
        VUsuarioNombre = e.VUsuarioNombre,
        ICodCliente    = e.ICodCliente,
        NombreCliente  = e.Cliente is not null
            ? $"{e.Cliente.CNombres} {e.Cliente.CApellidos}"
            : null,
        VTotal         = e.VTotal,
        RSaldo         = e.Reserva?.RSaldo,
        VSaldo         = e.VSaldo,
        Detalles       = e.Detalles?.Select(VentaDetalleDto.FromEntity).ToList() ?? new(),
        CreatedAt      = e.CreatedAt,
        UpdatedAt      = e.UpdatedAt,
        IsActive       = e.IsActive
    };
}
