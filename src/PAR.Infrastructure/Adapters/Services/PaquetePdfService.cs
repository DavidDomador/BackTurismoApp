using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PAR.Application.Ports;
using PAR.Infrastructure.Persistence;
using PAR.Shared.PDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PAR.Infrastructure.Adapters.Services;

public class PaquetePdfService(
    AppDbContext context,
    IHostEnvironment env,
    IPdfMergerService pdfMerger) : IPaquetePdfService
{
    private string PdfAdjuntosPath =>
        Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "paquetes-pdf");

    public async Task<byte[]> GenerarAsync(int iCodPaquete, CancellationToken ct = default)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var paquete = await context.Paquetes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ICodPaquete == iCodPaquete && p.IsActive, ct)
            ?? throw new InvalidOperationException("Paquete no encontrado.");

        var itinerarios = await context.Itinerarios
            .Where(i => i.ICodPaquete == iCodPaquete && i.IsActive)
            .OrderBy(i => i.INombre)
            .AsNoTracking()
            .ToListAsync(ct);

        var empresa = await context.Empresas
            .Where(e => e.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        // ── Si tiene PDF adjunto: fusionar adjunto + página de itinerarios ──
        if (!string.IsNullOrEmpty(paquete.PdfAdjunto))
        {
            var adjuntoPath = Path.Combine(PdfAdjuntosPath, paquete.PdfAdjunto);
            if (File.Exists(adjuntoPath))
            {
                var adjuntoBytes    = await File.ReadAllBytesAsync(adjuntoPath, ct);
                var itinerariosPage = GenerarPaginaItinerarios(paquete, itinerarios, empresa?.ERazonSocial);
                // Insertar itinerarios en la hoja 3 (índice base-0 = 2)
                return pdfMerger.InsertAt(adjuntoBytes, itinerariosPage, atPageIndex: 2);
            }
        }

        // ── Sin adjunto: documento completo (comportamiento original) ──
        return GenerarDocumentoCompleto(paquete, itinerarios, empresa);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Documento completo (sin PDF adjunto)
    // ─────────────────────────────────────────────────────────────────────────

    private byte[] GenerarDocumentoCompleto(
        Domain.Entities.Paquete paquete,
        List<Domain.Entities.Itinerario> itinerarios,
        Domain.Entities.Empresa? empresa)
    {
        var uploadsPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "itinerarios");

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(0);
                page.MarginVertical(0);
                page.DefaultTextStyle(t => t.FontFamily(PdfStyles.FontFamily).FontSize(PdfStyles.FontBase).FontColor(PdfStyles.TextPrimary));

                // ── CABECERA ─────────────────────────────────────────────
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
                               c.Item().Text("PAR TURISMO — PAQUETE")
                                       .FontSize(PdfStyles.FontXs).FontColor(PdfStyles.White)
                                       .Bold().LetterSpacing(0.08f);
                               c.Item().PaddingTop(6)
                                       .Text(paquete.PNombre)
                                       .FontSize(PdfStyles.FontTitle).Bold().FontColor(PdfStyles.White);
                               if (!string.IsNullOrWhiteSpace(paquete.PDescripcion))
                                   c.Item().PaddingTop(6).Text(paquete.PDescripcion)
                                           .FontSize(PdfStyles.FontSm).FontColor(PdfStyles.White);
                           });

                           row.ConstantItem(72).AlignMiddle().Column(badge =>
                               badge.Item().Width(64).Height(64)
                                    .Background(PdfStyles.PrimaryGreen).CornerRadius(32)
                                    .AlignCenter().AlignMiddle().Text("📦").FontSize(28));
                       });

                    col.Item().Height(6).Background(PdfStyles.PrimaryGreen);
                });

                // ── CONTENIDO ────────────────────────────────────────────
                page.Content()
                    .Background(PdfStyles.PageBg)
                    .PaddingHorizontal(32).PaddingTop(20).PaddingBottom(16)
                    .Column(col =>
                    {
                        AgregarPrecios(col, paquete);
                        AgregarItinerarios(col, itinerarios, uploadsPath);
                        AgregarNotaLegal(col);
                    });

                page.Footer().StandardFooter("PAR Turismo — Sistema de Gestión de Paquetes");
            });
        });

        return doc.GeneratePdf();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Página de itinerarios (para fusionar con PDF adjunto en hoja 3)
    // ─────────────────────────────────────────────────────────────────────────

    private byte[] GenerarPaginaItinerarios(
        Domain.Entities.Paquete paquete,
        List<Domain.Entities.Itinerario> itinerarios,
        string? empresaNombre)
    {
        var uploadsPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "itinerarios");

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);
                page.DefaultTextStyle(t => t.FontFamily(PdfStyles.FontFamily).FontSize(PdfStyles.FontBase).FontColor(PdfStyles.TextPrimary));

                // Sin cabecera ni pie — solo los itinerarios sobre fondo blanco
                page.Content()
                    .Background(PdfStyles.White)
                    .Column(col =>
                    {
                        AgregarItinerarios(col, itinerarios, uploadsPath);
                    });
            });
        });

        return doc.GeneratePdf();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers compartidos
    // ─────────────────────────────────────────────────────────────────────────

    private static void AgregarPrecios(ColumnDescriptor col, Domain.Entities.Paquete paquete)
    {
        col.SectionTitle("Precios del Paquete", PdfStyles.PrimaryGreen);
        col.Item().PaddingTop(8).Row(row =>
        {
            row.RelativeItem().DataBox("PRECIO BASE", $"S/ {paquete.PPrecioBase:N2}", PdfStyles.LightBlue, PdfStyles.PrimaryBlue);
            row.ConstantItem(12);

            if (paquete.PPrecioDescuento.HasValue)
            {
                row.RelativeItem().DataBox("PRECIO DESCUENTO", $"S/ {paquete.PPrecioDescuento:N2}", PdfStyles.LightGreen, PdfStyles.PrimaryGreen);
                row.ConstantItem(12);
            }

            if (paquete.PPrecioReserva.HasValue)
            {
                row.RelativeItem().DataBox("PRECIO RESERVA", $"S/ {paquete.PPrecioReserva:N2}", "#fff8e8", PdfStyles.AccentYellow);
                row.ConstantItem(12);
            }

            if (!paquete.PPrecioDescuento.HasValue && !paquete.PPrecioReserva.HasValue)
                row.RelativeItem();
        });
    }

    private static void AgregarItinerarios(
        ColumnDescriptor col,
        List<Domain.Entities.Itinerario> itinerarios,
        string uploadsPath)
    {
        if (itinerarios.Count == 0)
        {
            col.Divider("SIN ITINERARIOS");
            col.Item().PaddingVertical(16).AlignCenter()
               .Text("Este paquete no tiene itinerarios registrados.")
               .FontSize(PdfStyles.FontSm).FontColor(PdfStyles.TextSecondary).Italic();
            return;
        }

        // ── Título centrado, sin cantidad ────────────────────────────────────
        col.Item().PaddingBottom(12)
           .Background("#23b256").CornerRadius(6)
           .PaddingHorizontal(16).PaddingVertical(11)
           .AlignCenter()
           .Text("ITINERARIO")
           .FontFamily(PdfStyles.FontFamilyDisplay).FontSize(PdfStyles.ItinTitle).Bold()
           .FontColor(PdfStyles.White).LetterSpacing(0.06f);

        foreach (var (itin, idx) in itinerarios.Select((x, i) => (x, i)))
        {
            var imgPath  = !string.IsNullOrEmpty(itin.Imagen)
                ? Path.Combine(uploadsPath, itin.Imagen) : null;

            // Ícono: usa el del itinerario o map-pin por defecto
            var iconName = !string.IsNullOrEmpty(itin.Icon) ? itin.Icon : "map-pin";
            var iconSvg  = PAR.Shared.PDF.SvgIconLibrary.GetSvg(iconName, "#f77c00", 28)
                        ?? PAR.Shared.PDF.SvgIconLibrary.GetSvg("map-pin", "#f77c00", 28)!;

            var invertido = idx % 2 == 1; // par: icono|texto|img  impar: img|texto|icono

            col.Item().PaddingTop(idx == 0 ? 6 : 8).Row(row =>
            {
                if (!invertido)
                {
                    // ── Icono ───────────────────────────────────────────────
                    row.ConstantItem(44).AlignMiddle().AlignCenter()
                       .Width(28).Height(28)
                       .Svg(iconSvg);

                    // ── Contenido ────────────────────────────────────────────
                    row.RelativeItem().PaddingHorizontal(8).PaddingVertical(8).Column(c =>
                    {
                        c.Item().Text(itin.INombre)
                         .FontFamily(PdfStyles.FontFamilyDisplay)
                         .FontSize(PdfStyles.ItinName).Bold().FontColor("#2a6496");

                        if (!string.IsNullOrEmpty(itin.IDescripcion))
                            c.Item().PaddingTop(4).Text(itin.IDescripcion)
                             .FontFamily(PdfStyles.FontFamily)
                             .FontSize(PdfStyles.ItinDesc)
                             .FontColor(PdfStyles.TextSecondary).LineHeight(1.5f);
                    });

                    // ── Imagen circular ──────────────────────────────────────
                    row.ConstantItem(96).PaddingLeft(4).PaddingVertical(6).Column(img =>
                    {
                        if (imgPath is not null && File.Exists(imgPath))
                            img.Item().Width(80).Height(80).CornerRadius(40).Image(imgPath).FitArea();
                        else
                            img.Item().Width(80).Height(80).Background("#e8faf0")
                               .CornerRadius(40).AlignCenter().AlignMiddle()
                               .Text("🗺️").FontSize(24);
                    });
                }
                else
                {
                    // ── Imagen circular ──────────────────────────────────────
                    row.ConstantItem(96).PaddingRight(4).PaddingVertical(6).Column(img =>
                    {
                        if (imgPath is not null && File.Exists(imgPath))
                            img.Item().Width(80).Height(80).CornerRadius(40).Image(imgPath).FitArea();
                        else
                            img.Item().Width(80).Height(80).Background("#e8faf0")
                               .CornerRadius(40).AlignCenter().AlignMiddle()
                               .Text("🗺️").FontSize(24);
                    });

                    // ── Contenido ────────────────────────────────────────────
                    row.RelativeItem().PaddingHorizontal(8).PaddingVertical(8).Column(c =>
                    {
                        c.Item().Text(itin.INombre)
                         .FontFamily(PdfStyles.FontFamilyDisplay)
                         .FontSize(PdfStyles.ItinName).Bold().FontColor("#2a6496");

                        if (!string.IsNullOrEmpty(itin.IDescripcion))
                            c.Item().PaddingTop(4).Text(itin.IDescripcion)
                             .FontFamily(PdfStyles.FontFamily)
                             .FontSize(PdfStyles.ItinDesc)
                             .FontColor(PdfStyles.TextSecondary).LineHeight(1.5f);
                    });

                    // ── Icono ───────────────────────────────────────────────
                    row.ConstantItem(44).AlignMiddle().AlignCenter()
                       .Width(28).Height(28)
                       .Svg(iconSvg);
                }
            });
        }
    }

    private static void AgregarNotaLegal(ColumnDescriptor col)
    {
        col.Item().PaddingTop(20).PaddingBottom(4)
           .Background("#fffbeb").Border(1).BorderColor("#fde68a")
           .CornerRadius(6).Padding(10)
           .Row(r =>
           {
               r.AutoItem().PaddingRight(8).Text("ℹ️").FontSize(12);
               r.RelativeItem()
                .Text("Los precios mostrados son referenciales y pueden estar sujetos a cambios sin previo aviso. Para más información contacte a nuestro equipo de ventas.")
                .FontSize(PdfStyles.FontXs).FontColor("#92400e").LineHeight(1.4f);
           });
    }
}
