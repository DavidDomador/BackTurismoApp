using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PAR.Application.Ports;
using PAR.Infrastructure.Persistence;

namespace PAR.Infrastructure.Adapters.Services;

public class SalidaExcelService(AppDbContext context) : ISalidaExcelService
{
    // Paleta
    private const string TitleBg    = "#0f172a";
    private const string HeaderBg   = "#1e40af";
    private const string SubHdrBg   = "#3b5fc7";
    private const string AltRowBg   = "#f0f4ff";
    private const string SepBg      = "#dbeafe";
    private const string White      = "#ffffff";
    private const string TextDark   = "#0f172a";
    private const string TextMuted  = "#475569";

    public async Task<byte[]> GenerarAsync(CancellationToken ct = default)
    {
        // Carga completa con todas las relaciones necesarias
        var salidas = await context.Salidas
            .Where(s => s.IsActive)
            .Include(s => s.Guia)
            .Include(s => s.ChoferVehiculo).ThenInclude(cv => cv!.Chofer)
            .Include(s => s.ChoferVehiculo).ThenInclude(cv => cv!.Vehiculo)
            .Include(s => s.SalidaReservas)
                .ThenInclude(sr => sr.Reserva)
                    .ThenInclude(r => r!.Paquete)
            .OrderBy(s => s.FechaSalida).ThenBy(s => s.HoraSalida)
            .AsNoTracking()
            .ToListAsync(ct);

        // Cargar pasajeros de cada reserva involucrada
        var reservaIds = salidas
            .SelectMany(s => s.SalidaReservas.Select(sr => sr.ICodReserva))
            .Distinct()
            .ToList();

        var pasajerosPorReserva = await context.ReservaDetalles
            .Where(d => d.IsActive && reservaIds.Contains(d.ICodReserva))
            .Include(d => d.Cliente)
            .OrderBy(d => d.ICodReserva).ThenBy(d => d.ICodReservaDetalle)
            .AsNoTracking()
            .ToListAsync(ct);

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Salidas");

        int row = 1;

        // ── Título principal ─────────────────────────────────────────
        ws.Range(row, 1, row, 8).Merge();
        var titleCell = ws.Cell(row, 1);
        titleCell.Value = "REGISTRO DE SALIDAS — LISTA DE PASAJEROS";
        StyleTitle(titleCell);
        ws.Row(row).Height = 22;
        row++;

        // ── Fecha de impresión ───────────────────────────────────────
        ws.Range(row, 1, row, 8).Merge();
        var printCell = ws.Cell(row, 1);
        printCell.Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        printCell.Style.Font.FontSize = 9;
        printCell.Style.Font.FontColor = XLColor.FromHtml(TextMuted);
        printCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        ws.Row(row).Height = 14;
        row++;
        row++; // espacio

        foreach (var salida in salidas)
        {
            var guiaNombre    = salida.Guia is not null
                ? $"{salida.Guia.GNombre} {salida.Guia.GApellidos}"
                : "—";
            var placa         = salida.ChoferVehiculo?.Vehiculo?.GPlaca ?? "—";
            var marca         = salida.ChoferVehiculo?.Vehiculo is { } v
                ? $"{v.GMarca} {v.GModelo}"
                : "";
            var chofer        = salida.ChoferVehiculo?.Chofer is { } c
                ? $"{c.GNombre} {c.GApellidos}"
                : "—";
            var asientos      = salida.ChoferVehiculo?.Vehiculo?.GCantidadAsientos;

            // ── Cabecera de la salida ────────────────────────────────
            // Fila 1 de cabecera: etiquetas
            var hdrLabels = ws.Range(row, 1, row, 8);
            SetRowBg(hdrLabels, SubHdrBg);
            ws.Cell(row, 1).Value = "FECHA SALIDA";
            ws.Cell(row, 2).Value = "HORA";
            ws.Cell(row, 3).Value = "GUÍA";
            ws.Cell(row, 4).Value = "TRANSPORTE";
            ws.Cell(row, 5).Value = "CHOFER";
            ws.Cell(row, 6).Value = "RESERVAS";
            ws.Cell(row, 7).Value = "ASIENTOS";
            ws.Cell(row, 8).Value = "PASAJEROS TOTALES";
            foreach (var col in Enumerable.Range(1, 8))
                StyleSubHeader(ws.Cell(row, col));
            ws.Row(row).Height = 16;
            row++;

            // Fila 2 de cabecera: valores
            var totalPax = salida.SalidaReservas
                .Sum(sr => pasajerosPorReserva.Count(p => p.ICodReserva == sr.ICodReserva));
            var reservasNums = string.Join(", ",
                salida.SalidaReservas.Select(sr => $"R{sr.Reserva?.RNumeroReserva ?? sr.ICodReserva}"));

            ws.Cell(row, 1).Value = salida.FechaSalida.ToString("dd/MM/yyyy");
            ws.Cell(row, 2).Value = salida.HoraSalida.ToString(@"hh\:mm");
            ws.Cell(row, 3).Value = guiaNombre;
            ws.Cell(row, 4).Value = $"{placa}  {marca}".Trim();
            ws.Cell(row, 5).Value = chofer;
            ws.Cell(row, 6).Value = reservasNums;
            ws.Cell(row, 7).Value = asientos.HasValue ? asientos.Value.ToString() : "—";
            ws.Cell(row, 8).Value = totalPax;
            foreach (var col in Enumerable.Range(1, 8))
                StyleDataBold(ws.Cell(row, col), "#f8fafc");
            ws.Row(row).Height = 18;
            row++;
            row++; // espacio antes del detalle

            // ── Columnas de pasajeros ────────────────────────────────
            string[] paxHeaders = { "#", "Apellidos", "Nombres", "DNI", "Edad", "Reserva", "Paquete", "Fecha Tour" };
            for (int i = 0; i < paxHeaders.Length; i++)
            {
                var cell = ws.Cell(row, i + 1);
                cell.Value = paxHeaders[i];
                StyleHeader(cell);
            }
            ws.Row(row).Height = 16;
            row++;

            // ── Filas de pasajeros ───────────────────────────────────
            int paxNum = 1;
            foreach (var sr in salida.SalidaReservas)
            {
                var pasajeros = pasajerosPorReserva
                    .Where(p => p.ICodReserva == sr.ICodReserva)
                    .ToList();

                if (pasajeros.Count == 0)
                {
                    // Sin pasajeros registrados
                    ws.Range(row, 1, row, 8).Merge();
                    var emptyCell = ws.Cell(row, 1);
                    emptyCell.Value = $"  R{sr.Reserva?.RNumeroReserva ?? sr.ICodReserva} — {sr.Reserva?.Paquete?.PNombre ?? "Paquete no encontrado"} (sin pasajeros registrados)";
                    emptyCell.Style.Font.Italic = true;
                    emptyCell.Style.Font.FontColor = XLColor.FromHtml(TextMuted);
                    ws.Row(row).Height = 15;
                    row++;
                    continue;
                }

                bool isAlt = false;
                foreach (var p in pasajeros)
                {
                    string bg = isAlt ? AltRowBg : White;
                    ws.Cell(row, 1).Value = paxNum++;
                    ws.Cell(row, 2).Value = p.Cliente?.CApellidos ?? "—";
                    ws.Cell(row, 3).Value = p.Cliente?.CNombres ?? "—";
                    ws.Cell(row, 4).Value = p.Cliente?.CDni ?? "—";
                    ws.Cell(row, 5).Value = p.Cliente?.CEdad.HasValue == true
                        ? p.Cliente.CEdad.Value.ToString()
                        : "—";
                    ws.Cell(row, 6).Value = $"R{sr.Reserva?.RNumeroReserva ?? sr.ICodReserva}";
                    ws.Cell(row, 7).Value = sr.Reserva?.Paquete?.PNombre ?? "—";
                    ws.Cell(row, 8).Value = sr.Reserva?.FechaTour.ToString("dd/MM/yyyy") ?? "—";

                    for (int a = 1; a <= 8; a++)
                        StyleDataRow(ws.Cell(row, a), bg);

                    ws.Row(row).Height = 15;
                    row++;
                    isAlt = !isAlt;
                }
            }

            // ── Separador entre salidas ──────────────────────────────
            ws.Range(row, 1, row, 8).Merge();
            ws.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.FromHtml(SepBg);
            ws.Row(row).Height = 6;
            row++;
            row++;
        }

        // ── Ajuste final de columnas ─────────────────────────────────
        ws.Column(1).Width = 5;   // #
        ws.Column(2).Width = 22;  // Apellidos
        ws.Column(3).Width = 20;  // Nombres
        ws.Column(4).Width = 13;  // DNI
        ws.Column(5).Width = 7;   // Edad
        ws.Column(6).Width = 10;  // Reserva
        ws.Column(7).Width = 28;  // Paquete
        ws.Column(8).Width = 13;  // Fecha Tour

        ws.SheetView.FreezeRows(0);
        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        ws.PageSetup.PaperSize = XLPaperSize.A4Paper;
        ws.PageSetup.FitToPages(1, 0);
        ws.PageSetup.SetRowsToRepeatAtTop(1, 3);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // ── Helpers de estilo ────────────────────────────────────────────
    private static void StyleTitle(IXLCell cell)
    {
        cell.Style.Font.Bold = true;
        cell.Style.Font.FontSize = 13;
        cell.Style.Font.FontColor = XLColor.FromHtml(White);
        cell.Style.Fill.BackgroundColor = XLColor.FromHtml(TitleBg);
        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private static void StyleSubHeader(IXLCell cell)
    {
        cell.Style.Font.Bold = true;
        cell.Style.Font.FontSize = 8;
        cell.Style.Font.FontColor = XLColor.FromHtml(White);
        cell.Style.Fill.BackgroundColor = XLColor.FromHtml(SubHdrBg);
        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#93c5fd");
    }

    private static void StyleHeader(IXLCell cell)
    {
        cell.Style.Font.Bold = true;
        cell.Style.Font.FontSize = 9;
        cell.Style.Font.FontColor = XLColor.FromHtml(White);
        cell.Style.Fill.BackgroundColor = XLColor.FromHtml(HeaderBg);
        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#1e40af");
    }

    private static void StyleDataBold(IXLCell cell, string bg)
    {
        cell.Style.Font.Bold = true;
        cell.Style.Font.FontSize = 10;
        cell.Style.Font.FontColor = XLColor.FromHtml(TextDark);
        cell.Style.Fill.BackgroundColor = XLColor.FromHtml(bg);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#cbd5e1");
        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private static void StyleDataRow(IXLCell cell, string bg)
    {
        cell.Style.Font.FontSize = 9;
        cell.Style.Font.FontColor = XLColor.FromHtml(TextDark);
        cell.Style.Fill.BackgroundColor = XLColor.FromHtml(bg);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#e2e8f0");
        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private static void SetRowBg(IXLRange range, string color)
    {
        range.Style.Fill.BackgroundColor = XLColor.FromHtml(color);
    }
}
