namespace PAR.Application.Features.Empresa.DTOs;

public class EmpresaDto
{
    public int ICodEmpresa { get; set; }
    public string ERazonSocial { get; set; } = string.Empty;
    public string? ERUC { get; set; }
    public string? EDireccion { get; set; }
    public string? ETelefono { get; set; }
    public string? ECorreo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static EmpresaDto FromEntity(Domain.Entities.Empresa e) => new()
    {
        ICodEmpresa  = e.ICodEmpresa,
        ERazonSocial = e.ERazonSocial,
        ERUC         = e.ERUC,
        EDireccion   = e.EDireccion,
        ETelefono    = e.ETelefono,
        ECorreo      = e.ECorreo,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt,
        IsActive     = e.IsActive
    };
}
