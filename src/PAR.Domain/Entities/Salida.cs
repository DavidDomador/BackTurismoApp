namespace PAR.Domain.Entities;

public class Salida
{
    public int ICodSalida { get; set; }
    public int ICodigoGuia { get; set; }
    public int ICodChoferVehiculo { get; set; }
    public DateTime FechaSalida { get; set; }
    public TimeSpan HoraSalida { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<SalidaReserva> SalidaReservas { get; set; } = [];
    public Guia? Guia { get; set; }
    public ChoferVehiculo? ChoferVehiculo { get; set; }
}
