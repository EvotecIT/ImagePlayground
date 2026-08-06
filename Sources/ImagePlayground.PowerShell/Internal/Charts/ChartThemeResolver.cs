using CfxTheme = ChartForgeX.Themes.ChartTheme;

namespace ImagePlayground;

/// <summary>Maps the PowerShell-friendly theme selection to the shared ChartForgeX theme owner.</summary>
internal static class ChartThemeResolver {
    /// <summary>Creates a fresh ChartForgeX theme for the requested public ImagePlayground theme.</summary>
    public static CfxTheme Resolve(ChartTheme theme) {
        switch (theme) {
            case ChartTheme.Dark:
                return CfxTheme.ReportDark();
            case ChartTheme.Colorblind:
                return CfxTheme.Colorblind();
            case ChartTheme.Aurora:
                return CfxTheme.Aurora();
            case ChartTheme.Editorial:
                return CfxTheme.Editorial();
            case ChartTheme.Candy:
                return CfxTheme.Candy();
            case ChartTheme.PeopleInfographic:
                return CfxTheme.PeopleInfographic();
            case ChartTheme.Terminal:
                return CfxTheme.Terminal();
            case ChartTheme.TransparentOverlayDark:
                return CfxTheme.TransparentOverlayDark();
            case ChartTheme.Minimal:
                return CfxTheme.Minimal();
            case ChartTheme.DashboardLight:
                return CfxTheme.DashboardLight();
            case ChartTheme.SaasDashboardLight:
                return CfxTheme.SaasDashboardLight();
            case ChartTheme.RestaurantDashboardLight:
                return CfxTheme.RestaurantDashboardLight();
            default:
                return CfxTheme.ReportLight();
        }
    }
}
