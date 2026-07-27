using Microsoft.EntityFrameworkCore;
using PAR.Application.Ports;
using PAR.Infrastructure.Persistence;
using PAR.Shared.PDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PAR.Infrastructure.Adapters.Services;

public class VentaPdfService(AppDbContext context) : IVentaPdfService
{
    // Paleta monocromática para boleta seria
    private const string Ink        = "#1a1a1a";
    private const string InkMuted   = "#555555";
    private const string InkLight   = "#888888";
    private const string HeaderBg   = "#1a2b4b";
    private const string HeaderText = "#ffffff";
    private const string RowAlt     = "#f7f7f7";
    private const string BorderLine = "#cccccc";
    private const string White      = "#ffffff";
    private const string AccentLine = "#1a2b4b";

    public async Task<byte[]> GenerarAsync(int iCodVenta, CancellationToken ct = default)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var venta = await context.Ventas
            .Include(v => v.Reserva).ThenInclude(r => r!.Paquete)
            .Include(v => v.Cliente)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.ICodVenta == iCodVenta && v.IsActive, ct)
            ?? throw new InvalidOperationException("Venta no encontrada.");

        var detalles = venta.Reserva is not null
            ? await context.ReservaDetalles
                .Include(d => d.Cliente)
                .Include(d => d.EstadoClienteDetalle)
                .Where(d => d.ICodReserva == venta.ICodReserva && d.IsActive)
                .OrderBy(d => d.ICodReservaDetalle)
                .AsNoTracking()
                .ToListAsync(ct)
            : new();

        var empresa = await context.Empresas
            .Where(e => e.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        int pasajerosCount = detalles.Count;
        decimal tarifa     = venta.Reserva?.RTarifa ?? 0m;
        decimal rTotal     = tarifa * pasajerosCount;
        decimal rAbono     = venta.Reserva?.RAbono ?? 0m;
        decimal rSaldo     = rTotal - rAbono;

        string estadoTexto = "—";
        if (venta.Reserva?.Estado.HasValue == true)
        {
            var def = await context.DefinicionDetalles
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDefD == venta.Reserva.Estado.Value, ct);
            if (def is not null)
                estadoTexto = string.IsNullOrWhiteSpace(def.DdAbreviacion)
                    ? def.DdValue
                    : def.DdAbreviacion;
        }

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
                    .FontColor(Ink));

                // ── CABECERA ─────────────────────────────────────────
                page.Header().Column(col =>
                {
                    // Banda empresa (dark navy)
                    col.EmpresaHeader(empresa?.ERazonSocial, empresa?.ERUC, empresa?.EDireccion, empresa?.ETelefono, empresa?.ECorreo);

                    // Datos del documento
                    col.Item()
                       .Background(White)
                       .BorderBottom(2).BorderColor(AccentLine)
                       .PaddingHorizontal(32).PaddingTop(18).PaddingBottom(14)
                       .Row(row =>
                       {
                           row.RelativeItem().Column(c =>
                           {
                               c.Item()
                                .Text("BOLETA DE VENTA")
                                .FontSize(PdfStyles.FontXs).FontColor(InkLight)
                                .Bold().LetterSpacing(0.1f);

                               c.Item().PaddingTop(4)
                                .Text(venta.VNumeroVenta)
                                .FontSize(22f).Bold().FontColor(HeaderBg);

                               c.Item().PaddingTop(3)
                                .Text(venta.Reserva?.Paquete?.PNombre ?? "—")
                                .FontSize(PdfStyles.FontBase).FontColor(InkMuted);
                           });

                           row.ConstantItem(160).Column(info =>
                           {
                               void InfoRow(string label, string value) =>
                                   info.Item().PaddingBottom(3).Row(r =>
                                   {
                                       r.ConstantItem(70).Text(label)
                                        .FontSize(PdfStyles.FontXs).FontColor(InkLight).Bold();
                                       r.RelativeItem().Text(value)
                                        .FontSize(PdfStyles.FontXs).FontColor(Ink).Bold();
                                   });

                               InfoRow("FECHA:", venta.VFechaVenta.ToString("dd/MM/yyyy"));
                               InfoRow("RESERVA:", venta.Reserva?.RNumeroReserva.ToString() ?? "—");
                               InfoRow("TOUR:", venta.Reserva?.FechaTour.ToString("dd/MM/yyyy") ?? "—");
                               InfoRow("ESTADO:", estadoTexto);
                           });
                       });
                });

                // ── CONTENIDO ────────────────────────────────────────
                page.Content()
                    .Background(White)
                    .PaddingHorizontal(32).PaddingTop(18).PaddingBottom(16)
                    .Column(col =>
                    {
                        // ── Vendedor y cliente ─────────────────────────
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("VENDEDOR")
                                 .FontSize(PdfStyles.FontXs).FontColor(InkLight).Bold().LetterSpacing(0.06f);
                                c.Item().PaddingTop(2)
                                 .Text(venta.VUsuarioNombre)
                                 .FontSize(PdfStyles.FontBase).Bold().FontColor(Ink);
                            });

                            if (venta.Cliente is not null)
                            {
                                row.ConstantItem(16);
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("CLIENTE ENCARGADO")
                                     .FontSize(PdfStyles.FontXs).FontColor(InkLight).Bold().LetterSpacing(0.06f);
                                    c.Item().PaddingTop(2)
                                     .Text($"{venta.Cliente.CApellidos}, {venta.Cliente.CNombres}")
                                     .FontSize(PdfStyles.FontBase).Bold().FontColor(Ink);
                                    c.Item().PaddingTop(1)
                                     .Text($"DNI: {venta.Cliente.CDni}")
                                     .FontSize(PdfStyles.FontXs).FontColor(InkMuted);
                                });
                            }
                        });

                        // Línea separadora
                        col.Item().PaddingVertical(12).LineHorizontal(1).LineColor(BorderLine);

                        // ── Resumen económico ──────────────────────────
                        col.Item().Text("RESUMEN ECONÓMICO")
                           .FontSize(PdfStyles.FontXs).FontColor(InkLight).Bold().LetterSpacing(0.06f);

                        col.Item().PaddingTop(8)
                           .Border(1).BorderColor(BorderLine)
                           .Table(tbl =>
                           {
                               tbl.ColumnsDefinition(c =>
                               {
                                   c.RelativeColumn(3);
                                   c.RelativeColumn(2);
                                   c.RelativeColumn(2);
                                   c.RelativeColumn(2);
                               });

                               // Encabezado
                               void TH(string txt) =>
                                   tbl.Cell().Background(HeaderBg).Padding(8)
                                      .Text(txt).FontSize(PdfStyles.FontXs).Bold().FontColor(HeaderText);

                               TH("CONCEPTO"); TH("TARIFA/PAX"); TH("PASAJEROS"); TH("IMPORTE");

                               // Fila total
                               tbl.Cell().Padding(8)
                                  .Text(venta.Reserva?.Paquete?.PNombre ?? "Servicio de tour")
                                  .FontSize(PdfStyles.FontSm).FontColor(Ink);
                               tbl.Cell().Padding(8)
                                  .Text(tarifa > 0 ? $"S/ {tarifa:N2}" : "—")
                                  .FontSize(PdfStyles.FontSm).FontColor(Ink);
                               tbl.Cell().Padding(8)
                                  .Text(pasajerosCount.ToString())
                                  .FontSize(PdfStyles.FontSm).FontColor(Ink);
                               tbl.Cell().Padding(8)
                                  .Text($"S/ {rTotal:N2}")
                                  .FontSize(PdfStyles.FontSm).Bold().FontColor(Ink);
                           });

                        // Abono / saldo
                        col.Item().PaddingTop(6).Row(row =>
                        {
                            row.RelativeItem();
                            row.ConstantItem(260).Column(c =>
                            {
                                void SumRow(string label, string value, bool bold = false, bool last = false)
                                {
                                    c.Item()
                                     .BorderBottom(last ? 2 : 1).BorderColor(last ? AccentLine : BorderLine)
                                     .PaddingVertical(5).PaddingHorizontal(8)
                                     .Row(r =>
                                     {
                                         r.RelativeItem().Text(label)
                                          .FontSize(PdfStyles.FontSm).FontColor(InkMuted);
                                         var vt = r.AutoItem().Text(value)
                                          .FontSize(PdfStyles.FontSm)
                                          .FontColor(bold ? Ink : InkMuted);
                                         if (bold) vt.Bold();
                                     });
                                }

                                SumRow("SUBTOTAL", $"S/ {rTotal:N2}");
                                SumRow("ABONO", rAbono > 0 ? $"S/ {rAbono:N2}" : "S/ 0.00");
                                SumRow("SALDO PENDIENTE", $"S/ {rSaldo:N2}", bold: true, last: true);
                            });
                        });

                        // Línea separadora
                        col.Item().PaddingVertical(12).LineHorizontal(1).LineColor(BorderLine);

                        // ── Lista de pasajeros ─────────────────────────
                        col.Item().Text($"DETALLE DE PASAJEROS ({pasajerosCount})")
                           .FontSize(PdfStyles.FontXs).FontColor(InkLight).Bold().LetterSpacing(0.06f);

                        if (pasajerosCount == 0)
                        {
                            col.Item().PaddingVertical(14).AlignCenter()
                               .Text("Sin pasajeros registrados para esta reserva.")
                               .FontSize(PdfStyles.FontSm).FontColor(InkLight).Italic();
                        }
                        else
                        {
                            col.Item().PaddingTop(8)
                               .Border(1).BorderColor(BorderLine)
                               .Table(tbl =>
                               {
                                   tbl.ColumnsDefinition(c =>
                                   {
                                       c.ConstantColumn(26);
                                       c.RelativeColumn(4);
                                       c.RelativeColumn(2);
                                       c.RelativeColumn(2);
                                       c.RelativeColumn(2);
                                   });

                                   // Header
                                   void TH(string txt) =>
                                       tbl.Cell().Background(HeaderBg).Padding(7)
                                          .Text(txt).FontSize(PdfStyles.FontXs).Bold().FontColor(HeaderText);

                                   TH("#"); TH("APELLIDOS Y NOMBRES"); TH("DNI"); TH("ESTADO"); TH("TARIFA");

                                   foreach (var (det, idx) in detalles.Select((d, i) => (d, i)))
                                   {
                                       var bg = idx % 2 == 0 ? White : RowAlt;
                                       var cliente = det.Cliente;
                                       var nombre = cliente is not null
                                           ? $"{cliente.CApellidos}, {cliente.CNombres}"
                                           : "—";
                                       var dni = cliente?.CDni ?? "—";
                                       var estado = det.EstadoClienteDetalle is not null
                                           ? (string.IsNullOrWhiteSpace(det.EstadoClienteDetalle.DdAbreviacion)
                                               ? det.EstadoClienteDetalle.DdValue
                                               : det.EstadoClienteDetalle.DdAbreviacion)
                                           : "—";

                                       tbl.Cell().Background(bg).Padding(7)
                                          .Text($"{idx + 1}")
                                          .FontSize(PdfStyles.FontXs).FontColor(InkLight);
                                       tbl.Cell().Background(bg).Padding(7)
                                          .Text(nombre)
                                          .FontSize(PdfStyles.FontSm).Bold().FontColor(Ink);
                                       tbl.Cell().Background(bg).Padding(7)
                                          .Text(dni)
                                          .FontSize(PdfStyles.FontSm).FontColor(InkMuted);
                                       tbl.Cell().Background(bg).Padding(7)
                                          .Text(estado)
                                          .FontSize(PdfStyles.FontSm).FontColor(InkMuted);
                                       tbl.Cell().Background(bg).Padding(7)
                                          .Text(tarifa > 0 ? $"S/ {tarifa:N2}" : "—")
                                          .FontSize(PdfStyles.FontSm).FontColor(Ink);
                                   }
                               });
                        }
                    });

                // ── PIE DE PÁGINA ─────────────────────────────────────
                page.Footer().StandardFooter(
                    $"{empresa?.ERazonSocial ?? "PAR Turismo"} — Boleta {venta.VNumeroVenta}");
            });
        });

        return doc.GeneratePdf();
    }
}
