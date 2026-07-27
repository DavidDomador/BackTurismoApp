using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PAR.Shared.PDF;

/// <summary>
/// Guía de estilos reutilizable para todos los documentos PDF del sistema PAR.
/// Centraliza colores, fuentes, tamaños y helpers de layout.
/// </summary>
public static class PdfStyles
{
    // ── Colores de marca ──────────────────────────────────────────
    public const string PrimaryGreen  = "#05be50";
    public const string PrimaryBlue   = "#0032a0";
    public const string DarkBlue      = "#00247a";
    public const string LightGreen    = "#e8faf0";
    public const string LightBlue     = "#e8eef8";
    public const string PageBg        = "#f4f6fb";
    public const string TextPrimary   = "#0d1b3e";
    public const string TextSecondary = "#536080";
    public const string White         = "#ffffff";
    public const string BorderColor   = "#e6e6e6";
    public const string AccentYellow  = "#f59e0b";

    // ── Fuentes ──────────────────────────────────────────────────
    public const string FontFamily        = "Arial";
    public const string FontFamilyDisplay = "Georgia"; // títulos y nombres: elegante, serif
    public const float  FontBase          = 10f;
    public const float  FontSm            = 8.5f;
    public const float  FontXs            = 7.5f;
    public const float  FontLg            = 13f;
    public const float  FontXl            = 18f;
    public const float  FontTitle         = 24f;

    // ── Tamaños específicos para itinerarios ─────────────────────
    public const float  ItinTitle         = 16f;  // "ITINERARIO"
    public const float  ItinName          = 14f;  // nombre de cada parada
    public const float  ItinDesc          = 10f;  // descripción

    // ── Helpers de texto ─────────────────────────────────────────

    public static TextSpanDescriptor ApplyBase(this TextSpanDescriptor t) =>
        t.FontFamily(FontFamily).FontSize(FontBase).FontColor(TextPrimary);

    public static TextSpanDescriptor ApplyMuted(this TextSpanDescriptor t) =>
        t.FontFamily(FontFamily).FontSize(FontSm).FontColor(TextSecondary);

    // ── Sección con título lateral de color ──────────────────────

    public static void SectionTitle(this ColumnDescriptor col, string text, string color = PrimaryBlue)
    {
        col.Item().PaddingTop(16).PaddingBottom(6).Row(row =>
        {
            row.ConstantItem(4).Background(color);
            row.RelativeItem().PaddingLeft(10)
               .Text(text)
               .FontFamily(FontFamily).FontSize(FontLg).Bold()
               .FontColor(TextPrimary);
        });
    }

    // ── Caja de dato (label + valor) ─────────────────────────────

    public static void DataBox(
        this IContainer container,
        string label, string value,
        string bgColor = LightBlue, string accentColor = PrimaryBlue)
    {
        container
           .Background(bgColor)
           .Border(1).BorderColor(BorderColor)
           .CornerRadius(6)
           .Padding(10)
           .Column(c =>
           {
               c.Item().Text(label)
                .FontFamily(FontFamily).FontSize(FontXs).FontColor(accentColor).Bold();
               c.Item().PaddingTop(2).Text(value)
                .FontFamily(FontFamily).FontSize(FontLg).Bold().FontColor(TextPrimary);
           });
    }

    // ── Divisor con texto ─────────────────────────────────────────

    public static void Divider(this ColumnDescriptor col, string? text = null)
    {
        col.Item().PaddingVertical(8).Row(row =>
        {
            row.RelativeItem().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            if (text is not null)
            {
                row.AutoItem().PaddingHorizontal(8)
                   .Text(text)
                   .FontFamily(FontFamily).FontSize(FontXs)
                   .FontColor(TextSecondary).Bold();
                row.RelativeItem().PaddingTop(5).LineHorizontal(1).LineColor(BorderColor);
            }
        });
    }

    // ── Cabecera de empresa ───────────────────────────────────────

    public static void EmpresaHeader(this ColumnDescriptor col,
        string? razonSocial, string? ruc, string? direccion, string? telefono, string? correo)
    {
        if (string.IsNullOrWhiteSpace(razonSocial)) return;

        col.Item()
           .Background(DarkBlue)
           .PaddingHorizontal(32).PaddingVertical(12)
           .Row(row =>
           {
               row.RelativeItem().Column(c =>
               {
                   c.Item()
                    .Text(razonSocial)
                    .FontFamily(FontFamily).FontSize(FontLg).Bold()
                    .FontColor(White);

                   var info = new System.Collections.Generic.List<string>();
                   if (!string.IsNullOrWhiteSpace(ruc))       info.Add($"RUC: {ruc}");
                   if (!string.IsNullOrWhiteSpace(direccion)) info.Add(direccion);
                   if (!string.IsNullOrWhiteSpace(telefono))  info.Add($"Tel: {telefono}");
                   if (!string.IsNullOrWhiteSpace(correo))    info.Add(correo);

                   if (info.Count > 0)
                       c.Item().PaddingTop(3)
                        .Text(string.Join("   |   ", info))
                        .FontFamily(FontFamily).FontSize(FontXs)
                        .FontColor(White);
               });
           });
    }

    // ── Footer estándar ───────────────────────────────────────────

    public static void StandardFooter(this IContainer container, string appName = "PAR — Sistema de Gestión")
    {
        container
            .Background(PrimaryBlue)
            .PaddingHorizontal(32).PaddingVertical(10)
            .Row(row =>
            {
                row.RelativeItem()
                   .Text(appName)
                   .FontFamily(FontFamily).FontSize(FontXs).FontColor(White);
                row.AutoItem()
                   .Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                   .FontFamily(FontFamily).FontSize(FontXs).FontColor(White);
            });
    }
}
