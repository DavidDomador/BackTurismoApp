namespace PAR.Application.Features.ReservaDetalle.DTOs;

public class ReservaDetalleDto
{
    public int ICodReservaDetalle { get; set; }
    public int ICodReserva { get; set; }
    public int ICodCliente { get; set; }
    public string? ClienteNombres { get; set; }
    public string? ClienteApellidos { get; set; }
    public string? ClienteDni { get; set; }
    public int? EstadoCliente { get; set; }
    public string? EstadoClienteValor { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static ReservaDetalleDto FromEntity(Domain.Entities.ReservaDetalle e) => new()
    {
        ICodReservaDetalle   = e.ICodReservaDetalle,
        ICodReserva          = e.ICodReserva,
        ICodCliente          = e.ICodCliente,
        ClienteNombres       = e.Cliente?.CNombres,
        ClienteApellidos     = e.Cliente?.CApellidos,
        ClienteDni           = e.Cliente?.CDni,
        EstadoCliente        = e.EstadoCliente,
        EstadoClienteValor   = e.EstadoClienteDetalle?.DdValue,
        CreatedAt            = e.CreatedAt,
        UpdatedAt            = e.UpdatedAt,
        IsActive             = e.IsActive
    };
}
