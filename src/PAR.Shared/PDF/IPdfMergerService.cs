namespace PAR.Shared.PDF;

/// <summary>
/// Contrato genérico reutilizable para combinar documentos PDF.
/// Se puede usar desde cualquier módulo del sistema (Paquetes, Reservas, Ventas, etc.).
/// </summary>
public interface IPdfMergerService
{
    /// <summary>
    /// Concatena dos documentos PDF y devuelve el resultado en bytes.
    /// Las páginas del segundo PDF se añaden tras las del primero.
    /// </summary>
    byte[] Merge(byte[] firstPdf, byte[] secondPdf);

    /// <summary>
    /// Inserta las páginas de <paramref name="insertPdf"/> dentro de <paramref name="basePdf"/>
    /// a partir del índice de página indicado (base 0).
    /// Ejemplo: <c>atPageIndex = 2</c> → las páginas insertadas quedan en la hoja 3.
    /// Si el PDF base tiene menos páginas que <paramref name="atPageIndex"/>,
    /// las páginas se insertan al final.
    /// </summary>
    byte[] InsertAt(byte[] basePdf, byte[] insertPdf, int atPageIndex);
}
