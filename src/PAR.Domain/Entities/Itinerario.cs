namespace PAR.Domain.Entities;

public class Itinerario
{
    public int ICodItinerario { get; set; }
    public int ICodPaquete { get; set; }
    public string INombre { get; set; } = string.Empty;
    public string? IDescripcion { get; set; }
    public string? Icon { get; set; }
    public string? Imagen { get; set; }   // solo el nombre del archivo
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;

    public Paquete? Paquete { get; set; }
}
