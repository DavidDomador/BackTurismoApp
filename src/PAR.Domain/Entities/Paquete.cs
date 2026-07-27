namespace PAR.Domain.Entities;

public class Paquete
{
    public int ICodPaquete { get; set; }
    public string PNombre { get; set; } = string.Empty;
    public string? PDescripcion { get; set; }
    public decimal PPrecioBase { get; set; }
    public decimal? PPrecioDescuento { get; set; }
    public decimal? PPrecioReserva { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    /// <summary>Nombre del archivo PDF adjunto (almacenado en wwwroot/uploads/paquetes-pdf). Null si no tiene PDF.</summary>
    public string? PdfAdjunto { get; set; }
    public ICollection<Itinerario> Itinerarios { get; set; } = new List<Itinerario>();
}
