namespace PAR.Domain.Entities;

public class ReservaDetalle
{
    public int ICodReservaDetalle { get; set; }
    public int ICodReserva { get; set; }
    public int ICodCliente { get; set; }
    public int? EstadoCliente { get; set; }          // IdDefD de DefinicionDetalle
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;

    public Reserva? Reserva { get; set; }
    public Cliente? Cliente { get; set; }
    public DefinicionDetalle? EstadoClienteDetalle { get; set; }
}
