namespace PAR.Domain.Entities;

public class SalidaReserva
{
    public int ICodSalidaReserva { get; set; }
    public int ICodSalida { get; set; }
    public int ICodReserva { get; set; }

    // Navigation
    public Salida? Salida { get; set; }
    public Reserva? Reserva { get; set; }
}
