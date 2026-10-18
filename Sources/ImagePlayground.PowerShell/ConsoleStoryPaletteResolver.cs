using System;
using ChartForgeX.Terminal;
using ChartForgeX.Themes;

namespace ImagePlayground.PowerShell;

internal static class ConsoleStoryPaletteResolver {
    internal static VisualTheme Resolve(string name) {
        switch ((name ?? string.Empty).ToUpperInvariant()) {
            case "DARK":
            case "LIGHT": return VisualTheme.Graphite();
            case "POWERSHELL": return TerminalThemes.PowerShell();
            case "WINDOWSPOWERSHELL": return TerminalThemes.WindowsPowerShell();
            case "UBUNTU": return TerminalThemes.Ubuntu();
            case "CAMPBELL": return TerminalThemes.Campbell();
            case "CLASSIC": return TerminalThemes.Classic();
            default: throw new InvalidOperationException("Unknown console story palette.");
        }
    }

    internal static VisualThemeMode ResolveMode(string name) => string.Equals(name, "Light", StringComparison.OrdinalIgnoreCase)
        ? VisualThemeMode.Light : VisualThemeMode.Dark;
}
