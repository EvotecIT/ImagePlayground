using ScottPlot;

namespace ImagePlayground;

/// <summary>Extension methods for ScottPlot.Plot.</summary>
internal static class PlotExtensions {
    /// <summary>Show or hide grid lines.</summary>
    /// <param name="plot">Plot instance.</param>
    /// <param name="show">Whether to show grid.</param>
    public static void ShowGrid(this Plot plot, bool show) {
        if (show) {
            plot.ShowGrid();
        } else {
            plot.HideGrid();
        }
    }
}
