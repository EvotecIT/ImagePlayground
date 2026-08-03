using System;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

internal static class ConsoleStoryPaletteResolver {
    internal static TerminalTheme Resolve(string name) {
        switch ((name ?? string.Empty).ToUpperInvariant()) {
            case "DARK": return TerminalTheme.Dark();
            case "POWERSHELL": return TerminalTheme.PowerShell();
            case "WINDOWSPOWERSHELL": return TerminalTheme.WindowsPowerShell();
            case "UBUNTU": return TerminalTheme.Ubuntu();
            case "CAMPBELL": return TerminalTheme.Campbell();
            case "CLASSIC": return TerminalTheme.Classic();
            case "LIGHT": return TerminalTheme.Light();
            default: throw new InvalidOperationException("Unknown console story palette.");
        }
    }
}
