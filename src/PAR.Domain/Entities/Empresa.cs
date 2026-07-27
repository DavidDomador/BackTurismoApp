namespace PAR.Domain.Entities;

public class Empresa
{
    public int ICodEmpresa { get; set; }
    public string ERazonSocial { get; set; } = string.Empty;
    public string? ERUC { get; set; }
    public string? EDireccion { get; set; }
    public string? ETelefono { get; set; }
    public string? ECorreo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
}
