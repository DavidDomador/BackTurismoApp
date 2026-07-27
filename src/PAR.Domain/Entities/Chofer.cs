namespace PAR.Domain.Entities;

public class Chofer
{
    public int ICodigoChofer { get; set; }
    public string GNombre { get; set; } = string.Empty;
    public string GApellidos { get; set; } = string.Empty;
    public string GDni { get; set; } = string.Empty;
    public string GLicencia { get; set; } = string.Empty;
    public string? GTelefono { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ChoferVehiculo> ChoferVehiculos { get; set; } = new List<ChoferVehiculo>();
}
