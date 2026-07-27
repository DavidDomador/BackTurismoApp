namespace PAR.Domain.Entities;

public class Guia
{
    public int ICodigoGuia { get; set; }
    public string GNombre { get; set; } = string.Empty;
    public string GApellidos { get; set; } = string.Empty;
    public string GDni { get; set; } = string.Empty;
    public string GCorreo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
}
