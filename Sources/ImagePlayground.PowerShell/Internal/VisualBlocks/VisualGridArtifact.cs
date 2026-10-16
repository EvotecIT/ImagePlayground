using System.Globalization;
using System.Text.RegularExpressions;
using ChartForgeX.VisualArtifacts;
using ChartForgeX.VisualBlocks;

namespace ImagePlayground.PowerShell;

/// <summary>Wraps a ChartForgeX visual grid in a visual artifact.</summary>
/// <remarks>ChartForgeX 2.0.0 offers ToVisualArtifact for charts, chart grids, canvases, and blocks but not for VisualGrid.
/// The grid is a native static visual source, so the artifact renders it directly without rasterizing it into a canvas.</remarks>
internal static class VisualGridArtifact {
    private static readonly Regex RootSize = new("<svg\\b[^>]*?\\bwidth=\"(?<w>[0-9.]+)\"[^>]*?\\bheight=\"(?<h>[0-9.]+)\"", RegexOptions.CultureInvariant);

    public static VisualArtifact Create(VisualGrid grid, string? id) {
        var artifact = VisualArtifact.Create(string.IsNullOrWhiteSpace(id) ? "visual-grid" : id!.Trim(), VisualArtifactKind.ChartGrid, grid);
        artifact.SourceLanguage = VisualArtifactSourceLanguage.Native;
        artifact.Title = grid.Title;
        artifact.Subtitle = grid.Subtitle;
        artifact.ExportFormats = VisualArtifactExportFormat.Svg | VisualArtifactExportFormat.Png |
                                 VisualArtifactExportFormat.Html | VisualArtifactExportFormat.Office;
        artifact.Metadata["render.model"] = nameof(VisualGrid);
        artifact.Metadata["visual-grid.items"] = grid.Items.Count.ToString(CultureInfo.InvariantCulture);
        artifact.Metadata["visual-grid.columns"] = grid.Columns.ToString(CultureInfo.InvariantCulture);
        if (grid.Title.Length > 0) {
            artifact.Accessibility.Name = grid.Title;
        }

        var match = RootSize.Match(grid.RenderSvg(string.Empty));
        if (match.Success &&
            double.TryParse(match.Groups["w"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var width) &&
            double.TryParse(match.Groups["h"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var height) &&
            width > 0 && height > 0) {
            artifact.NaturalSize = new VisualArtifactSize(width, height);
        } else {
            var image = grid.RenderRgba();
            artifact.NaturalSize = new VisualArtifactSize(image.Width / (double)grid.PngOutputScale, image.Height / (double)grid.PngOutputScale);
        }

        return artifact;
    }
}
