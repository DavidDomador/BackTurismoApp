namespace PAR.Application.Features.Guia.DTOs;

public class GuiaDto
{
    public int    ICodigoGuia     { get; set; }
    public string GNombre         { get; set; } = string.Empty;
    public string GApellidos      { get; set; } = string.Empty;
    public string GDni            { get; set; } = string.Empty;
    public string GCorreo         { get; set; } = string.Empty;
    public DateTime CreatedAt     { get; set; }
    public DateTime? UpdatedAt    { get; set; }
    public bool IsActive          { get; set; }
}
