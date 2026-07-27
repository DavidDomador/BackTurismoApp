using PAR.Domain.Entities;

namespace PAR.Application.Features.Transporte.DTOs;

public class ChoferDto
{
    public int    ICodigoChofer { get; set; }
    public string GNombre      { get; set; } = string.Empty;
    public string GApellidos   { get; set; } = string.Empty;
    public string GDni         { get; set; } = string.Empty;
    public string GLicencia    { get; set; } = string.Empty;
    public string? GTelefono   { get; set; }
}

public class VehiculoDto
{
    public int    ICodigoVehiculo { get; set; }
    public string GPlaca          { get; set; } = string.Empty;
    public string GMarca          { get; set; } = string.Empty;
    public string GModelo         { get; set; } = string.Empty;
    public int?   GAnio                { get; set; }
    public string? GColor              { get; set; }
    public int?   GCantidadAsientos   { get; set; }
}

public class TransporteDto
{
    public int         ICodigoChoferVehiculo { get; set; }
    public DateTime    FechaAsignacion       { get; set; }
    public bool        IsActive              { get; set; }
    public DateTime    CreatedAt             { get; set; }
    public ChoferDto   Chofer                { get; set; } = null!;
    public VehiculoDto Vehiculo              { get; set; } = null!;

    public static TransporteDto FromEntity(ChoferVehiculo cv) => new()
    {
        ICodigoChoferVehiculo = cv.ICodigoChoferVehiculo,
        FechaAsignacion       = cv.FechaAsignacion,
        IsActive              = cv.IsActive,
        CreatedAt             = cv.CreatedAt,
        Chofer = new ChoferDto
        {
            ICodigoChofer = cv.Chofer.ICodigoChofer,
            GNombre       = cv.Chofer.GNombre,
            GApellidos    = cv.Chofer.GApellidos,
            GDni          = cv.Chofer.GDni,
            GLicencia     = cv.Chofer.GLicencia,
            GTelefono     = cv.Chofer.GTelefono
        },
        Vehiculo = new VehiculoDto
        {
            ICodigoVehiculo = cv.Vehiculo.ICodigoVehiculo,
            GPlaca          = cv.Vehiculo.GPlaca,
            GMarca          = cv.Vehiculo.GMarca,
            GModelo         = cv.Vehiculo.GModelo,
            GAnio              = cv.Vehiculo.GAnio,
            GColor             = cv.Vehiculo.GColor,
            GCantidadAsientos  = cv.Vehiculo.GCantidadAsientos
        }
    };
}
