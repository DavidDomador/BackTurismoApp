namespace PAR.Domain.Entities;

public class Reserva
{
    public int ICodReserva { get; set; }
    public int RNumeroReserva { get; set; }
    public int ICodPaquete { get; set; }
    public DateTime FechaTour { get; set; }
    public decimal RTotal { get; set; }
    public decimal? RAbono { get; set; }
    public decimal? RSaldo { get; set; }
    public string? RIncluye { get; set; }
    public string? RNoIncluye { get; set; }
    public decimal? RTarifa { get; set; }
    public string? RObservacion { get; set; }
    public int? Estado { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;

    public Paquete? Paquete { get; set; }
    public ICollection<ReservaDetalle>? ReservaDetalles { get; set; }
}
