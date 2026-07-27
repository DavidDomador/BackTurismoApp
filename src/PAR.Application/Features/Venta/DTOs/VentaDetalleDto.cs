namespace PAR.Application.Features.Venta.DTOs;

public class VentaDetalleDto
{
    public int ICodVentaDetalle { get; set; }
    public int ICodReservaDetalle { get; set; }
    public int? ICodCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? DNI { get; set; }
    public decimal VMonto { get; set; }

    public static VentaDetalleDto FromEntity(Domain.Entities.VentaDetalle e) => new()
    {
        ICodVentaDetalle  = e.ICodVentaDetalle,
        ICodReservaDetalle = e.ICodReservaDetalle,
        ICodCliente       = e.ICodCliente,
        NombreCliente     = e.Cliente is not null
            ? $"{e.Cliente.CApellidos}, {e.Cliente.CNombres}"
            : null,
        DNI               = e.Cliente?.CDni,
        VMonto            = e.VMonto
    };
}
