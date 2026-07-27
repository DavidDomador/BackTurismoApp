using Microsoft.EntityFrameworkCore;
using PAR.Application.Ports;
using PAR.Infrastructure.Persistence;
using PAR.Shared.PDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PAR.Infrastructure.Adapters.Services;

public class ReservaPdfService(AppDbContext context) : IReservaPdfService
{
    public async Task<byte[]> GenerarAsync(int iCodReserva, CancellationToken ct = default)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var reserva = await context.Reservas
            .Include(r => r.Paquete)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ICodReserva == iCodReserva && r.IsActive, ct)
            ?? throw new InvalidOperationException("Reserva no encontrada.");

        var detalles = await context.ReservaDetalles
            .Include(d => d.Cliente)
            .Include(d => d.EstadoClienteDetalle)
            .Where(d => d.ICodReserva == iCodReserva && d.IsActive)
            .OrderBy(d => d.ICodReservaDetalle)
            .AsNoTracking()
            .ToListAsync(ct);

        // Texto del estado de la reserva
        string estadoTexto = "—";
        if (reserva.Estado.HasValue)
        {
            var def = await context.DefinicionDetalles
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDefD == reserva.Estado.Value, ct);
            if (def is not null)
                estadoTexto = string.IsNullOrWhiteSpace(def.DdAbreviacion)
                    ? def.DdValue
                    : $"{def.DdAbreviacion} — {def.DdValue}";
        }

        int pasajerosCount = detalles.Count;
        decimal rTotal = reserva.RTarifa.HasValue ? reserva.RTarifa.Value * pasajerosCount : 0m;
        decimal rSaldo = rTotal - (reserva.RAbono ?? 0m);

        var empresa = await context.Empresas
            .Where(e => e.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(0);
                page.MarginVertical(0);
                page.DefaultTextStyle(t => t
                    .FontFamily(PdfStyles.FontFamily)
                    .FontSize(PdfStyles.FontBase)
                    .FontColor(PdfStyles.TextPrimary));

                // ── CABECERA ─────────────────────────────────────────
                page.Header().Column(col =>
                {
                    col.EmpresaHeader(empresa?.ERazonSocial, empresa?.ERUC, empresa?.EDireccion, empresa?.ETelefono, empresa?.ECorreo);

                    col.Item()
                       .Background(PdfStyles.PrimaryBlue)
                       .PaddingHorizontal(32).PaddingTop(28).PaddingBottom(20)
                       .Row(row =>
                       {
                           row.RelativeItem().Column(c =>
                           {
                               c.Item()
                                .Text("PAR TURISMO — VOUCHER DE RESERVA")
                                .FontSize(PdfStyles.FontXs).FontColor(PdfStyles.White)
                                .Bold().LetterSpacing(0.08f);

                               c.Item().PaddingTop(6)
                                .Text($"# {reserva.RNumeroReserva}")
                                .FontSize(PdfStyles.FontTitle).Bold()
                                .FontColor(PdfStyles.White);

                               c.Item().PaddingTop(4)
                                .Text(reserva.Paquete?.PNombre ?? "—")
                                .FontSize(PdfStyles.FontLg)
                                .FontColor(PdfStyles.White);
                           });

                           // Fecha + estado
                           row.ConstantItem(140).AlignMiddle().Column(badge =>
                           {
                               badge.Item()
                                    .Background(PdfStyles.PrimaryGreen)
                                    .CornerRadius(6).Padding(10)
                                    .Column(c =>
                                    {
                                        c.Item()
                                         .Text("FECHA DEL TOUR")
                                         .FontSize(PdfStyles.FontXs)
                                         .FontColor(PdfStyles.White)
                                         .Bold().LetterSpacing(0.06f);
                                        c.Item().PaddingTop(3)
                                         .Text(reserva.FechaTour.ToString("dd/MM/yyyy"))
                                         .FontSize(PdfStyles.FontLg).Bold()
                                         .FontColor(PdfStyles.White);
                                        c.Item().PaddingTop(6)
                                         .Text("ESTADO")
                                         .FontSize(PdfStyles.FontXs)
                                         .FontColor(PdfStyles.White)
                                         .Bold().LetterSpacing(0.06f);
                                        c.Item().PaddingTop(2)
                                         .Text(estadoTexto)
                                         .FontSize(PdfStyles.FontSm)
                                         .FontColor(PdfStyles.White);
                                    });
                           });
                       });

                    col.Item().Height(6).Background(PdfStyles.PrimaryGreen);
                });

                // ── CONTENIDO ────────────────────────────────────────
                page.Content()
                    .Background(PdfStyles.PageBg)
                    .PaddingHorizontal(32).PaddingTop(20).PaddingBottom(16)
                    .Column(col =>
                    {
                        // ── Resumen económico ──────────────────────────
                        col.SectionTitle("Resumen Económico", PdfStyles.PrimaryBlue);
                        col.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().DataBox(
                                "TARIFA POR PAX",
                                reserva.RTarifa.HasValue ? $"S/ {reserva.RTarifa.Value:N2}" : "—",
                                PdfStyles.LightBlue, PdfStyles.PrimaryBlue);
                            row.ConstantItem(10);
                            row.RelativeItem().DataBox(
                                "PASAJEROS",
                                pasajerosCount.ToString(),
                                PdfStyles.LightBlue, PdfStyles.PrimaryBlue);
                            row.ConstantItem(10);
                            row.RelativeItem().DataBox(
                                "TOTAL",
                                $"S/ {rTotal:N2}",
                                PdfStyles.LightGreen, PdfStyles.PrimaryGreen);
                        });
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().DataBox(
                                "ABONO",
                                reserva.RAbono.HasValue ? $"S/ {reserva.RAbono.Value:N2}" : "S/ 0.00",
                                "#fff8e8", PdfStyles.AccentYellow);
                            row.ConstantItem(10);
                            row.RelativeItem().DataBox(
                                "SALDO PENDIENTE",
                                $"S/ {rSaldo:N2}",
                                rSaldo <= 0 ? PdfStyles.LightGreen : "#fce8ea",
                                rSaldo <= 0 ? PdfStyles.PrimaryGreen : "#b91c3a");
                            row.ConstantItem(10);
                            row.RelativeItem(); // espacio vacío para alinear
                        });

                        // ── Incluye / No incluye / Observaciones ──────
                        bool hasExtras = !string.IsNullOrWhiteSpace(reserva.RIncluye)
                                      || !string.IsNullOrWhiteSpace(reserva.RNoIncluye)
                                      || !string.IsNullOrWhiteSpace(reserva.RObservacion);

                        if (hasExtras)
                        {
                            col.SectionTitle("Información Adicional", PdfStyles.PrimaryBlue);

                            if (!string.IsNullOrWhiteSpace(reserva.RIncluye))
                            {
                                col.Item().PaddingTop(8)
                                   .Background(PdfStyles.LightGreen)
                                   .Border(1).BorderColor(PdfStyles.PrimaryGreen)
                                   .CornerRadius(6).Padding(10)
                                   .Column(c =>
                                   {
                                       c.Item().Text("INCLUYE")
                                        .FontSize(PdfStyles.FontXs).Bold()
                                        .FontColor(PdfStyles.PrimaryGreen);
                                       c.Item().PaddingTop(4).Text(reserva.RIncluye)
                                        .FontSize(PdfStyles.FontSm)
                                        .FontColor(PdfStyles.TextPrimary)
                                        .LineHeight(1.4f);
                                   });
                            }

                            if (!string.IsNullOrWhiteSpace(reserva.RNoIncluye))
                            {
                                col.Item().PaddingTop(8)
                                   .Background("#fce8ea")
                                   .Border(1).BorderColor("#b91c3a")
                                   .CornerRadius(6).Padding(10)
                                   .Column(c =>
                                   {
                                       c.Item().Text("NO INCLUYE")
                                        .FontSize(PdfStyles.FontXs).Bold()
                                        .FontColor("#b91c3a");
                                       c.Item().PaddingTop(4).Text(reserva.RNoIncluye)
                                        .FontSize(PdfStyles.FontSm)
                                        .FontColor(PdfStyles.TextPrimary)
                                        .LineHeight(1.4f);
                                   });
                            }

                            if (!string.IsNullOrWhiteSpace(reserva.RObservacion))
                            {
                                col.Item().PaddingTop(8)
                                   .Background("#fffbeb")
                                   .Border(1).BorderColor("#fde68a")
                                   .CornerRadius(6).Padding(10)
                                   .Column(c =>
                                   {
                                       c.Item().Text("OBSERVACIONES")
                                        .FontSize(PdfStyles.FontXs).Bold()
                                        .FontColor(PdfStyles.AccentYellow);
                                       c.Item().PaddingTop(4).Text(reserva.RObservacion)
                                        .FontSize(PdfStyles.FontSm)
                                        .FontColor(PdfStyles.TextPrimary)
                                        .LineHeight(1.4f);
                                   });
                            }
                        }

                        // ── Lista de pasajeros ─────────────────────────
                        col.SectionTitle($"Pasajeros ({pasajerosCount})", PdfStyles.PrimaryBlue);

                        if (pasajerosCount == 0)
                        {
                            col.Item().PaddingVertical(16)
                               .AlignCenter()
                               .Text("Sin pasajeros registrados para esta reserva.")
                               .FontSize(PdfStyles.FontSm)
                               .FontColor(PdfStyles.TextSecondary).Italic();
                        }
                        else
                        {
                            // Cabecera de tabla
                            col.Item().PaddingTop(8)
                               .Background(PdfStyles.PrimaryBlue)
                               .CornerRadius(4)
                               .Padding(8)
                               .Row(header =>
                               {
                                   header.ConstantItem(28)
                                         .Text("#")
                                         .FontSize(PdfStyles.FontXs).Bold()
                                         .FontColor(PdfStyles.White);
                                   header.RelativeItem(3)
                                         .Text("APELLIDOS Y NOMBRES")
                                         .FontSize(PdfStyles.FontXs).Bold()
                                         .FontColor(PdfStyles.White);
                                   header.RelativeItem(2)
                                         .Text("DNI")
                                         .FontSize(PdfStyles.FontXs).Bold()
                                         .FontColor(PdfStyles.White);
                                   header.RelativeItem(2)
                                         .Text("ESTADO")
                                         .FontSize(PdfStyles.FontXs).Bold()
                                         .FontColor(PdfStyles.White);
                               });

                            // Filas
                            foreach (var (det, idx) in detalles.Select((d, i) => (d, i)))
                            {
                                var rowBg = idx % 2 == 0 ? PdfStyles.White : PdfStyles.LightBlue;
                                var cliente = det.Cliente;
                                var nombre = cliente is not null
                                    ? $"{cliente.CApellidos}, {cliente.CNombres}"
                                    : "—";
                                var dni = cliente?.CDni ?? "—";
                                var estadoCliente = det.EstadoClienteDetalle is not null
                                    ? (string.IsNullOrWhiteSpace(det.EstadoClienteDetalle.DdAbreviacion)
                                        ? det.EstadoClienteDetalle.DdValue
                                        : det.EstadoClienteDetalle.DdAbreviacion)
                                    : "—";

                                col.Item()
                                   .Background(rowBg)
                                   .Border(1).BorderColor(PdfStyles.BorderColor)
                                   .Padding(8)
                                   .Row(row =>
                                   {
                                       row.ConstantItem(28)
                                          .Text($"{idx + 1}")
                                          .FontSize(PdfStyles.FontSm)
                                          .FontColor(PdfStyles.TextSecondary);
                                       row.RelativeItem(3)
                                          .Text(nombre)
                                          .FontSize(PdfStyles.FontSm).Bold()
                                          .FontColor(PdfStyles.TextPrimary);
                                       row.RelativeItem(2)
                                          .Text(dni)
                                          .FontSize(PdfStyles.FontSm)
                                          .FontColor(PdfStyles.TextSecondary);
                                       row.RelativeItem(2)
                                          .Text(estadoCliente)
                                          .FontSize(PdfStyles.FontSm)
                                          .FontColor(PdfStyles.TextSecondary);
                                   });
                            }
                        }
                    });

                // ── PIE DE PÁGINA ─────────────────────────────────────
                page.Footer().StandardFooter($"PAR Turismo — Voucher #{reserva.RNumeroReserva}");
            });
        });

        return doc.GeneratePdf();
    }
}
