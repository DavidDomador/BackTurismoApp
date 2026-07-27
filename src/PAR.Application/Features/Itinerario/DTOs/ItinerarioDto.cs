namespace PAR.Application.Features.Itinerario.DTOs;

public class ItinerarioDto
{
    public int ICodItinerario { get; set; }
    public int ICodPaquete { get; set; }
    public string INombre { get; set; } = string.Empty;
    public string? IDescripcion { get; set; }
    public string? Icon { get; set; }
    public string? Imagen { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static ItinerarioDto FromEntity(Domain.Entities.Itinerario e) => new()
    {
        ICodItinerario = e.ICodItinerario,
        ICodPaquete    = e.ICodPaquete,
        INombre        = e.INombre,
        IDescripcion   = e.IDescripcion,
        Icon           = e.Icon,
        Imagen         = e.Imagen,
        CreatedAt      = e.CreatedAt,
        UpdatedAt      = e.UpdatedAt,
        IsActive       = e.IsActive
    };
}
