using ChartForgeX.Themes;

namespace ImagePlayground;

/// <summary>Maps the PowerShell-friendly preset selection to the shared paired theme.</summary>
internal static class ChartThemeResolver {
    /// <summary>Resolves the immutable shared theme for a public preset.</summary>
    public static VisualTheme Resolve(ChartTheme theme) => theme switch {
        ChartTheme.Colorblind => VisualTheme.Colorblind(),
        ChartTheme.Aurora => VisualTheme.Aurora(),
        ChartTheme.Editorial => VisualTheme.Editorial(),
        ChartTheme.Candy => VisualTheme.Candy(),
        ChartTheme.PeopleInfographic => VisualTheme.PeopleInfographic(),
        ChartTheme.Terminal => VisualTheme.Terminal(),
        ChartTheme.TransparentOverlayDark => VisualTheme.TransparentOverlay(),
        ChartTheme.Minimal => VisualTheme.Minimal(),
        ChartTheme.DashboardLight => VisualTheme.Dashboard(),
        ChartTheme.SaasDashboardLight => VisualTheme.SaasDashboard(),
        ChartTheme.RestaurantDashboardLight => VisualTheme.RestaurantDashboard(),
        _ => VisualTheme.Graphite()
    };

    /// <summary>Resolves the authored mode without inferring it from the palette.</summary>
    public static VisualThemeMode ResolveMode(ChartTheme theme) => theme switch {
        ChartTheme.Dark or ChartTheme.Aurora or ChartTheme.Terminal or ChartTheme.TransparentOverlayDark => VisualThemeMode.Dark,
        _ => VisualThemeMode.Light
    };
}
