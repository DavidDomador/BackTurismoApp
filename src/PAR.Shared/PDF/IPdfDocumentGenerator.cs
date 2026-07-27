namespace PAR.Shared.PDF;

/// <summary>
/// Contrato genérico reutilizable para generadores de documentos PDF.
/// Permite añadir nuevos tipos de documentos sin cambiar la interfaz de consumo.
/// </summary>
public interface IPdfDocumentGenerator<in TModel>
{
    /// <summary>Genera el PDF y devuelve sus bytes.</summary>
    byte[] Generate(TModel model);
}
