namespace PAR.Domain.Entities;

public class Definicion
{
    public int IdDef { get; set; }
    public string DNombre { get; set; } = string.Empty;
    public string? DDescripcion { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<DefinicionDetalle> Detalles { get; set; } = new List<DefinicionDetalle>();
}
