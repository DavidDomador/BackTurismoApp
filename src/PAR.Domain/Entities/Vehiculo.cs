namespace PAR.Domain.Entities;

public class Vehiculo
{
    public int ICodigoVehiculo { get; set; }
    public string GPlaca { get; set; } = string.Empty;
    public string GMarca { get; set; } = string.Empty;
    public string GModelo { get; set; } = string.Empty;
    public int? GAnio { get; set; }
    public string? GColor { get; set; }
    public int? GCantidadAsientos { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ChoferVehiculo> ChoferVehiculos { get; set; } = new List<ChoferVehiculo>();
}
