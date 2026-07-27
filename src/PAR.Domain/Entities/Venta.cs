namespace PAR.Domain.Entities;

public class Venta
{
    public int ICodVenta { get; set; }
    public string VNumeroVenta { get; set; } = string.Empty;
    public int ICodReserva { get; set; }
    public DateTime VFechaVenta { get; set; }
    public int ICodUsuario { get; set; }
    public string VUsuarioNombre { get; set; } = string.Empty;
    public int? ICodCliente { get; set; }
    public decimal VTotal { get; set; }
    public decimal? VSaldo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;

    public Reserva? Reserva { get; set; }
    public Cliente? Cliente { get; set; }
    public ICollection<VentaDetalle>? Detalles { get; set; }
}
