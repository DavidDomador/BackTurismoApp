using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PAR.Shared.PDF;

namespace PAR.Infrastructure.Adapters.Services;

/// <summary>
/// Implementación de <see cref="IPdfMergerService"/> basada en PdfSharpCore.
/// </summary>
public class PdfMergerService : IPdfMergerService
{
    /// <inheritdoc/>
    public byte[] Merge(byte[] firstPdf, byte[] secondPdf)
    {
        var outputDoc = new PdfDocument();
        AppendPages(outputDoc, firstPdf);
        AppendPages(outputDoc, secondPdf);
        return Save(outputDoc);
    }

    /// <inheritdoc/>
    public byte[] InsertAt(byte[] basePdf, byte[] insertPdf, int atPageIndex)
    {
        var outputDoc = new PdfDocument();

        using var baseMs = new MemoryStream(basePdf);
        var baseDoc = PdfReader.Open(baseMs, PdfDocumentOpenMode.Import);

        using var insertMs = new MemoryStream(insertPdf);
        var insertDoc = PdfReader.Open(insertMs, PdfDocumentOpenMode.Import);

        // Número real de páginas antes del punto de inserción
        var splitAt = Math.Min(atPageIndex, baseDoc.PageCount);

        // Páginas del PDF base ANTES del punto de inserción
        for (var i = 0; i < splitAt; i++)
            outputDoc.AddPage(baseDoc.Pages[i]);

        // Páginas a insertar (itinerarios, etc.)
        for (var i = 0; i < insertDoc.PageCount; i++)
            outputDoc.AddPage(insertDoc.Pages[i]);

        // Páginas del PDF base DESPUÉS del punto de inserción
        for (var i = splitAt; i < baseDoc.PageCount; i++)
            outputDoc.AddPage(baseDoc.Pages[i]);

        return Save(outputDoc);
    }

    private static void AppendPages(PdfDocument target, byte[] pdfBytes)
    {
        using var ms = new MemoryStream(pdfBytes);
        var source = PdfReader.Open(ms, PdfDocumentOpenMode.Import);
        for (var i = 0; i < source.PageCount; i++)
            target.AddPage(source.Pages[i]);
    }

    private static byte[] Save(PdfDocument doc)
    {
        using var ms = new MemoryStream();
        doc.Save(ms, false);
        return ms.ToArray();
    }
}
