using PAR.Domain.Entities;

namespace PAR.Application.Features.Definicion.DTOs;

public class DefinicionDto
{
    public int IdDef { get; set; }
    public string DNombre { get; set; } = string.Empty;
    public string? DDescripcion { get; set; }
    public bool Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public IList<DefinicionDetalleDto> Detalles { get; set; } = [];

    public static DefinicionDto FromEntity(Domain.Entities.Definicion e) => new()
    {
        IdDef        = e.IdDef,
        DNombre      = e.DNombre,
        DDescripcion = e.DDescripcion,
        Estado       = e.Estado,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt,
        IsActive     = e.IsActive,
        Detalles     = e.Detalles.Select(DefinicionDetalleDto.FromEntity).ToList()
    };
}

public class DefinicionDetalleDto
{
    public int IdDefD { get; set; }
    public string DdValue { get; set; } = string.Empty;
    public int IdDef { get; set; }
    public string? DdAbreviacion { get; set; }
    public string? DdDescripcion { get; set; }
    public bool Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static DefinicionDetalleDto FromEntity(DefinicionDetalle e) => new()
    {
        IdDefD       = e.IdDefD,
        DdValue      = e.DdValue,
        IdDef        = e.IdDef,
        DdAbreviacion = e.DdAbreviacion,
        DdDescripcion = e.DdDescripcion,
        Estado       = e.Estado,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt,
        IsActive     = e.IsActive
    };
}
