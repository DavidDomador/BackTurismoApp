namespace PAR.Application.Features.Reserva.DTOs;

public class ReservaDto
{
    public int ICodReserva { get; set; }
    public int RNumeroReserva { get; set; }
    public int ICodPaquete { get; set; }
    public string? PaqueteNombre { get; set; }
    public DateTime FechaTour { get; set; }
    public decimal? RTarifa { get; set; }
    public int PasajerosCount { get; set; }
    public decimal RTotal { get; set; }
    public decimal? RAbono { get; set; }
    public decimal RSaldo { get; set; }
    public string? RIncluye { get; set; }
    public string? RNoIncluye { get; set; }
    public string? RObservacion { get; set; }
    public int? Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static ReservaDto FromEntity(Domain.Entities.Reserva e)
    {
        var pasajerosCount = e.ReservaDetalles?.Count(d => d.IsActive) ?? 0;
        var rTotal         = e.RTarifa.HasValue ? e.RTarifa.Value * pasajerosCount : 0m;
        var rSaldo         = rTotal - (e.RAbono ?? 0m);

        return new ReservaDto
        {
            ICodReserva    = e.ICodReserva,
            RNumeroReserva = e.RNumeroReserva,
            ICodPaquete    = e.ICodPaquete,
            PaqueteNombre  = e.Paquete?.PNombre,
            FechaTour      = e.FechaTour,
            RTarifa        = e.RTarifa,
            PasajerosCount = pasajerosCount,
            RTotal         = rTotal,
            RAbono         = e.RAbono,
            RSaldo         = rSaldo,
            RIncluye       = e.RIncluye,
            RNoIncluye     = e.RNoIncluye,
            RObservacion   = e.RObservacion,
            Estado         = e.Estado,
            CreatedAt      = e.CreatedAt,
            UpdatedAt      = e.UpdatedAt,
            IsActive       = e.IsActive
        };
    }
}
