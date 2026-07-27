namespace PAR.Shared.Excel;

/// <summary>Puerto de salida para generación de archivos Excel.</summary>
public interface IExcelExportService
{
    /// <param name="sheetName">Nombre de la hoja.</param>
    /// <param name="headers">Encabezados de columnas.</param>
    /// <param name="rows">Filas de datos (cada fila es un arreglo de valores).</param>
    byte[] Generate(string sheetName, string[] headers, IEnumerable<object?[]> rows);
}
