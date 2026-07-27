namespace PAR.Domain.Entities;

public class DefinicionDetalle
{
    public int IdDefD { get; set; }
    public string DdValue { get; set; } = string.Empty;
    public int IdDef { get; set; }
    public string? DdAbreviacion { get; set; }
    public string? DdDescripcion { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public Definicion? Definicion { get; set; }
}
