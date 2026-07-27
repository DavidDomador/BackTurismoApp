namespace PAR.Domain.Entities;

public class VentaDetalle
{
    public int ICodVentaDetalle { get; set; }
    public int ICodVenta { get; set; }
    public int ICodReservaDetalle { get; set; }
    public int? ICodCliente { get; set; }
    public decimal VMonto { get; set; }
    public bool IsActive { get; set; } = true;

    public Venta? Venta { get; set; }
    public ReservaDetalle? ReservaDetalle { get; set; }
    public Cliente? Cliente { get; set; }
}
