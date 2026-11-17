namespace ImagePlayground;

/// <summary>
/// Available chart themes.
/// </summary>
/// <remarks>Legacy presentation names select the canonical Graphite theme in their original light or dark mode.</remarks>
public enum ChartTheme
{
    /// <summary>Use the canonical light ChartForgeX colors, typography, and frame style.</summary>
    Default,
    /// <summary>Apply the canonical dark ChartForgeX theme.</summary>
    Dark,
    /// <summary>Apply the canonical light ChartForgeX theme.</summary>
    Light,
    /// <summary>Apply the Accessible colorblind-friendly Graphite palette.</summary>
    Colorblind,
    /// <summary>Apply the canonical dark theme; retained as an Aurora compatibility alias.</summary>
    Aurora,
    /// <summary>Apply the canonical light theme; retained as an editorial compatibility alias.</summary>
    Editorial,
    /// <summary>Apply the canonical light theme; retained as a candy compatibility alias.</summary>
    Candy,
    /// <summary>Apply the canonical light theme; retained as a people infographic compatibility alias.</summary>
    PeopleInfographic,
    /// <summary>Apply the canonical dark theme; retained as a terminal compatibility alias.</summary>
    Terminal,
    /// <summary>Apply the transparent dark overlay theme.</summary>
    TransparentOverlayDark,
    /// <summary>Apply the canonical light theme; retained as a minimal compatibility alias.</summary>
    Minimal,
    /// <summary>Apply the canonical light theme; retained as a dashboard compatibility alias.</summary>
    DashboardLight,
    /// <summary>Apply the canonical light theme; retained as a SaaS dashboard compatibility alias.</summary>
    SaasDashboardLight,
    /// <summary>Apply the canonical light theme; retained as a restaurant dashboard compatibility alias.</summary>
    RestaurantDashboardLight
}
