namespace PAR.Domain.Entities;

public class Cliente
{
    public int ICodCliente { get; set; }
    public string CNombres { get; set; } = string.Empty;
    public string CApellidos { get; set; } = string.Empty;
    public string CDni { get; set; } = string.Empty;
    public string? CCorreo { get; set; }
    public int? CEdad { get; set; }
    public string? CDireccion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
}
