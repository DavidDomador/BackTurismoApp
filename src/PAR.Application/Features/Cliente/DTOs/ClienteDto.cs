namespace PAR.Application.Features.Cliente.DTOs;

public class ClienteDto
{
    public int ICodCliente { get; set; }
    public string CNombres { get; set; } = string.Empty;
    public string CApellidos { get; set; } = string.Empty;
    public string CDni { get; set; } = string.Empty;
    public string? CCorreo { get; set; }
    public int? CEdad { get; set; }
    public string? CDireccion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static ClienteDto FromEntity(Domain.Entities.Cliente e) => new()
    {
        ICodCliente  = e.ICodCliente,
        CNombres     = e.CNombres,
        CApellidos   = e.CApellidos,
        CDni         = e.CDni,
        CCorreo      = e.CCorreo,
        CEdad        = e.CEdad,
        CDireccion   = e.CDireccion,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt,
        IsActive     = e.IsActive
    };
}
