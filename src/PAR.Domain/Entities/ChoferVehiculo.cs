namespace PAR.Domain.Entities;

public class ChoferVehiculo
{
    public int ICodigoChoferVehiculo { get; set; }
    public int ChoferID { get; set; }
    public int VehiculoID { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? CreatedByUserId { get; set; }
    public Chofer Chofer { get; set; } = null!;
    public Vehiculo Vehiculo { get; set; } = null!;
}
