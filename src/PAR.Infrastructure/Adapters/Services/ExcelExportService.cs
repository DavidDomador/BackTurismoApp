using ClosedXML.Excel;
using PAR.Shared.Excel;

namespace PAR.Infrastructure.Adapters.Services;

public class ExcelExportService : IExcelExportService
{
    private const string HeaderBg   = "#0032a0";
    private const string AltRowBg   = "#eef2fa";

    public byte[] Generate(string sheetName, string[] headers, IEnumerable<object?[]> rows)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(sheetName);

        // ── Encabezado ────────────────────────────────────────────
        for (int c = 0; c < headers.Length; c++)
        {
            var cell = ws.Cell(1, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(HeaderBg);
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#c8d0e0");
        }

        // ── Filas de datos ────────────────────────────────────────
        int rowNum = 2;
        foreach (var row in rows)
        {
            bool isAlt = rowNum % 2 == 0;
            for (int c = 0; c < row.Length; c++)
            {
                var cell = ws.Cell(rowNum, c + 1);
                var val  = row[c];

                if (val is DateTime dt)       cell.Value = dt.ToString("dd/MM/yyyy");
                else if (val is decimal dec)  cell.Value = dec;
                else if (val is int intVal)   cell.Value = intVal;
                else if (val is bool boolVal) cell.Value = boolVal ? "Sí" : "No";
                else                          cell.Value = val?.ToString() ?? "";

                if (isAlt)
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml(AltRowBg);

                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#e6e6e6");
            }
            rowNum++;
        }

        // ── Formato final ─────────────────────────────────────────
        if (rowNum > 2)
            ws.RangeUsed()?.SetAutoFilter();

        ws.Columns().AdjustToContents(8, 60);
        ws.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
