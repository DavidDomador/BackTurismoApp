namespace PAR.Shared.PDF;

/// <summary>
/// Biblioteca de íconos SVG (Lucide Icons) accesibles por nombre.
/// Usar nombres como "map-pin", "camera", "hotel" en el campo Icon del itinerario.
/// </summary>
public static class SvgIconLibrary
{
    /// <summary>
    /// Retorna el SVG completo listo para usar con QuestPDF (.Svg()), o null si el nombre no existe.
    /// </summary>
    public static string? GetSvg(string? name, string color = "#f77c00", int size = 20)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        if (!Icons.TryGetValue(name.Trim().ToLower(), out var paths)) return null;

        return $"""
            <svg xmlns="http://www.w3.org/2000/svg" width="{size}" height="{size}"
                 viewBox="0 0 24 24" fill="none" stroke="{color}"
                 stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              {paths}
            </svg>
            """;
    }

    /// <summary>Retorna true si el nombre de ícono existe en la biblioteca.</summary>
    public static bool Exists(string? name) =>
        !string.IsNullOrWhiteSpace(name) && Icons.ContainsKey(name.Trim().ToLower());

    // ── Catálogo ──────────────────────────────────────────────────────────────
    public static IEnumerable<string> Names => Icons.Keys;

    private static readonly Dictionary<string, string> Icons = new()
    {
        // Lugares / navegación
        ["map-pin"]      = """<path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/>""",
        ["map"]          = """<polygon points="1 6 1 22 8 18 16 22 23 18 23 2 16 6 8 2 1 6"/><line x1="8" y1="2" x2="8" y2="18"/><line x1="16" y1="6" x2="16" y2="22"/>""",
        ["compass"]      = """<circle cx="12" cy="12" r="10"/><polygon points="16.24 7.76 14.12 14.12 7.76 16.24 9.88 9.88 16.24 7.76"/>""",
        ["navigation"]   = """<polygon points="3 11 22 2 13 21 11 13 3 11"/>""",
        ["landmark"]     = """<line x1="3" y1="22" x2="21" y2="22"/><line x1="6" y1="18" x2="6" y2="11"/><line x1="10" y1="18" x2="10" y2="11"/><line x1="14" y1="18" x2="14" y2="11"/><line x1="18" y1="18" x2="18" y2="11"/><polygon points="12 2 20 7 4 7"/>""",

        // Transporte
        ["plane"]        = """<path d="M17.8 19.2 16 11l3.5-3.5C21 6 21.5 4 21 3c-1-.5-3 0-4.5 1.5L13 8 4.8 6.2c-.5-.1-.9.1-1.1.5l-.3.5c-.2.5-.1 1 .3 1.3L9 12l-2 3H4l-1 1 3 2 2 3 1-1v-3l3-2 3.5 5.3c.3.4.8.5 1.3.3l.5-.2c.4-.3.6-.7.5-1.2z"/>""",
        ["bus"]          = """<path d="M8 6v6"/><path d="M15 6v6"/><path d="M2 12h19.6"/><path d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3"/><circle cx="7" cy="18" r="2"/><path d="M9 18h5"/><circle cx="16" cy="18" r="2"/>""",
        ["train"]        = """<rect x="4" y="3" width="16" height="16" rx="2"/><path d="M4 11h16"/><path d="M12 3v8"/><path d="m8 19-2 3"/><path d="m18 22-2-3"/><path d="M8 15h.01"/><path d="M16 15h.01"/>""",
        ["ship"]         = """<path d="M2 21c.6.5 1.2 1 2.5 1 2.5 0 2.5-2 5-2 1.3 0 1.9.5 2.5 1 .6.5 1.2 1 2.5 1 2.5 0 2.5-2 5-2 1.3 0 1.9.5 2.5 1"/><path d="M19.38 20A11.6 11.6 0 0 0 21 14l-9-4-9 4c0 2.9.94 5.34 2.81 7.76"/><path d="M19 13V7a2 2 0 0 0-2-2H7a2 2 0 0 0-2 2v6"/><path d="M12 10v4"/><path d="M12 3v4"/>""",
        ["bike"]         = """<circle cx="18.5" cy="17.5" r="3.5"/><circle cx="5.5" cy="17.5" r="3.5"/><circle cx="15" cy="5" r="1"/><path d="M12 17.5V14l-3-3 4-3 2 3h2"/>""",
        ["anchor"]       = """<circle cx="12" cy="5" r="3"/><line x1="12" y1="22" x2="12" y2="8"/><path d="M5 12H2a10 10 0 0 0 20 0h-3"/>""",

        // Naturaleza
        ["mountain"]     = """<path d="m8 3 4 8 5-5 5 15H2L8 3z"/>""",
        ["tree"]         = """<path d="M17 14h.01"/><path d="M7 7h.01"/><path d="M3 6l9-4 9 4-4.5 3.5L21 13l-9 4-9-4 4.5-3.5L3 6z"/><path d="M12 22V9"/>""",
        ["sun"]          = """<circle cx="12" cy="12" r="4"/><path d="M12 2v2"/><path d="M12 20v2"/><path d="m4.93 4.93 1.41 1.41"/><path d="m17.66 17.66 1.41 1.41"/><path d="M2 12h2"/><path d="M20 12h2"/><path d="m6.34 17.66-1.41 1.41"/><path d="m19.07 4.93-1.41 1.41"/>""",
        ["moon"]         = """<path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>""",
        ["umbrella"]     = """<path d="M23 12a11.05 11.05 0 0 0-22 0zm-5 7a3 3 0 0 1-6 0v-7"/>""",
        ["tent"]         = """<path d="M3.5 21 14 3"/><path d="M20.5 21 10 3"/><path d="M15.5 21 12 15l-3.5 6"/><path d="M2 21h20"/>""",
        ["flower"]       = """<circle cx="12" cy="12" r="3"/><path d="M12 2a4 4 0 0 1 4 4 4 4 0 0 1 4-4 4 4 0 0 1-4 4 4 4 0 0 1 4 4 4 4 0 0 1-4-4 4 4 0 0 1-4 4 4 4 0 0 1 4-4 4 4 0 0 1-4-4 4 4 0 0 1 4 4z"/>""",
        ["fish"]         = """<path d="M6.5 12c.94-3.46 4.94-6 8.5-6 3.56 0 6.06 2.54 7 6-.94 3.47-3.44 6-7 6s-7.56-2.53-8.5-6z"/><path d="M18 12v.5"/><path d="M16 17.93a9.77 9.77 0 0 1 0-11.86"/><path d="M7 10.67C7 8 5.58 5.97 2.73 5.5c-1 3.54-.07 7.23 2.27 9.5C7.58 18.03 9 16 9 13.33"/><path d="m11.33 6.89 1.45 1.45"/><path d="m11.33 17.11 1.45-1.45"/>""",
        ["waves"]        = """<path d="M2 6c.6.5 1.2 1 2.5 1C7 7 7 5 9.5 5c2.6 0 2.4 2 5 2 2.5 0 2.5-2 5-2 1.3 0 1.9.5 2.5 1"/><path d="M2 12c.6.5 1.2 1 2.5 1 2.5 0 2.5-2 5-2 2.6 0 2.4 2 5 2 2.5 0 2.5-2 5-2 1.3 0 1.9.5 2.5 1"/><path d="M2 18c.6.5 1.2 1 2.5 1 2.5 0 2.5-2 5-2 2.6 0 2.4 2 5 2 2.5 0 2.5-2 5-2 1.3 0 1.9.5 2.5 1"/>""",

        // Alojamiento / servicios
        ["hotel"]        = """<path d="M18 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V4a2 2 0 0 0-2-2z"/><path d="M9 22V12h6v10"/><path d="M8 7h.01"/><path d="M16 7h.01"/><path d="M12 7h.01"/><path d="M12 11h.01"/><path d="M16 11h.01"/><path d="M8 11h.01"/>""",
        ["home"]         = """<path d="m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/>""",
        ["utensils"]     = """<path d="M3 2v7c0 1.1.9 2 2 2h4a2 2 0 0 0 2-2V2"/><path d="M7 2v20"/><path d="M21 15V2v0a5 5 0 0 0-5 5v6c0 1.1.9 2 2 2h3Zm0 0v7"/>""",
        ["coffee"]       = """<path d="M17 8h1a4 4 0 1 1 0 8h-1"/><path d="M3 8h14v9a4 4 0 0 1-4 4H7a4 4 0 0 1-4-4Z"/><line x1="6" y1="2" x2="6" y2="4"/><line x1="10" y1="2" x2="10" y2="4"/><line x1="14" y1="2" x2="14" y2="4"/>""",
        ["wine"]         = """<path d="M8 22h8"/><path d="M7 10h10"/><path d="M12 15v7"/><path d="M12 15a5 5 0 0 0 5-5c0-2-.5-4-2-8H9c-1.5 4-2 6-2 8a5 5 0 0 0 5 5z"/>""",
        ["shopping-bag"] = """<path d="M6 2 3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/>""",

        // Actividades / entretenimiento
        ["camera"]       = """<path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/><circle cx="12" cy="13" r="4"/>""",
        ["music"]        = """<path d="M9 18V5l12-2v13"/><circle cx="6" cy="18" r="3"/><circle cx="18" cy="16" r="3"/>""",
        ["star"]         = """<polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/>""",
        ["heart"]        = """<path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>""",
        ["zap"]          = """<polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2"/>""",
    };
}
