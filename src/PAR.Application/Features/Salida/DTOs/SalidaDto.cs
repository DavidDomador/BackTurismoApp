namespace PAR.Application.Features.Salida.DTOs;

public class SalidaDto
{
    public int ICodSalida { get; set; }
    public int ICodigoGuia { get; set; }
    public string? NombreGuia { get; set; }
    public int ICodChoferVehiculo { get; set; }
    public string? NombreChofer { get; set; }
    public string? PlacaVehiculo { get; set; }
    public DateTime FechaSalida { get; set; }
    public TimeSpan HoraSalida { get; set; }
    public List<SalidaReservaDto> Reservas { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static SalidaDto FromEntity(Domain.Entities.Salida e) => new()
    {
        ICodSalida         = e.ICodSalida,
        ICodigoGuia        = e.ICodigoGuia,
        NombreGuia         = e.Guia is not null
            ? $"{e.Guia.GNombre} {e.Guia.GApellidos}"
            : null,
        ICodChoferVehiculo = e.ICodChoferVehiculo,
        NombreChofer       = e.ChoferVehiculo?.Chofer is not null
            ? $"{e.ChoferVehiculo.Chofer.GNombre} {e.ChoferVehiculo.Chofer.GApellidos}"
            : null,
        PlacaVehiculo      = e.ChoferVehiculo?.Vehiculo?.GPlaca,
        FechaSalida        = e.FechaSalida,
        HoraSalida         = e.HoraSalida,
        Reservas           = e.SalidaReservas
            .Select(SalidaReservaDto.FromEntity)
            .ToList(),
        CreatedAt          = e.CreatedAt,
        UpdatedAt          = e.UpdatedAt,
        IsActive           = e.IsActive
    };
}

public class SalidaReservaDto
{
    public int ICodSalidaReserva { get; set; }
    public int ICodReserva { get; set; }
    public int? RNumeroReserva { get; set; }
    public string? NombrePaquete { get; set; }
    public DateTime? FechaTour { get; set; }

    public static SalidaReservaDto FromEntity(Domain.Entities.SalidaReserva sr) => new()
    {
        ICodSalidaReserva = sr.ICodSalidaReserva,
        ICodReserva       = sr.ICodReserva,
        RNumeroReserva    = sr.Reserva?.RNumeroReserva,
        NombrePaquete     = sr.Reserva?.Paquete?.PNombre,
        FechaTour         = sr.Reserva?.FechaTour
    };
}
