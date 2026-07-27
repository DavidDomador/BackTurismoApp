namespace PAR.Application.Features.Paquete.DTOs;

public class PaqueteDto
{
    public int ICodPaquete { get; set; }
    public string PNombre { get; set; } = string.Empty;
    public string? PDescripcion { get; set; }
    public decimal PPrecioBase { get; set; }
    public decimal? PPrecioDescuento { get; set; }
    public decimal? PPrecioReserva { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public string? PdfAdjunto { get; set; }

    public static PaqueteDto FromEntity(Domain.Entities.Paquete e) => new()
    {
        ICodPaquete      = e.ICodPaquete,
        PNombre          = e.PNombre,
        PDescripcion     = e.PDescripcion,
        PPrecioBase      = e.PPrecioBase,
        PPrecioDescuento = e.PPrecioDescuento,
        PPrecioReserva   = e.PPrecioReserva,
        CreatedAt        = e.CreatedAt,
        UpdatedAt        = e.UpdatedAt,
        IsActive         = e.IsActive,
        PdfAdjunto       = e.PdfAdjunto
    };
}
