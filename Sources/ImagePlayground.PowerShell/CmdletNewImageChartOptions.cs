using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates renderer options for New-ImageChart.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartOptions")]
[OutputType(typeof(ChartRenderOptions))]
public sealed class NewImageChartOptionsCmdlet : PSCmdlet {
    /// <summary>Palette colors used by series and point-based charts.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color[]? Palette { get; set; }

    /// <summary>Show the legend.</summary>
    [Parameter]
    public SwitchParameter ShowLegend { get; set; }

    /// <summary>Hide the legend.</summary>
    [Parameter]
    public SwitchParameter NoLegend { get; set; }

    /// <summary>Show point-level legend entries.</summary>
    [Parameter]
    public SwitchParameter ShowPointLegend { get; set; }

    /// <summary>Legend placement.</summary>
    [Parameter]
    public ChartLegendPosition LegendPosition { get; set; } = ChartLegendPosition.Bottom;

    /// <summary>Hide axes.</summary>
    [Parameter]
    public SwitchParameter NoAxes { get; set; }

    /// <summary>Hide chart card surface.</summary>
    [Parameter]
    public SwitchParameter NoCard { get; set; }

    /// <summary>Hide plot background surface.</summary>
    [Parameter]
    public SwitchParameter NoPlotBackground { get; set; }

    /// <summary>Use transparent background.</summary>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <summary>Show data labels.</summary>
    [Parameter]
    public SwitchParameter ShowDataLabels { get; set; }

    /// <summary>Preferred number of axis ticks.</summary>
    [Parameter]
    public int TickCount { get; set; }

    /// <summary>Heatmap color scale.</summary>
    [Parameter]
    public ChartHeatmapColorScale HeatmapScale { get; set; } = ChartHeatmapColorScale.Sequential;

    /// <summary>Hide heatmap scale legend.</summary>
    [Parameter]
    public SwitchParameter NoHeatmapScale { get; set; }

    /// <summary>Pie and donut slice label content.</summary>
    [Parameter]
    public ChartPieLabelContent PieLabelContent { get; set; } = ChartPieLabelContent.Label;

    /// <summary>Donut inner radius ratio.</summary>
    [Parameter]
    public double DonutInnerRadiusRatio { get; set; }

    /// <summary>Donut center value text.</summary>
    [Parameter]
    public string DonutCenterValue { get; set; } = string.Empty;

    /// <summary>Donut center label text.</summary>
    [Parameter]
    public string DonutCenterLabel { get; set; } = string.Empty;

    /// <summary>Progress maximum.</summary>
    [Parameter]
    public double ProgressMaximum { get; set; }

    /// <summary>Hide progress handles.</summary>
    [Parameter]
    public SwitchParameter NoProgressHandles { get; set; }

    /// <summary>Pictorial symbol.</summary>
    [Parameter]
    public ChartPictorialSymbol PictorialSymbol { get; set; } = ChartPictorialSymbol.Circle;

    /// <summary>Pictorial columns per row.</summary>
    [Parameter]
    public int PictorialColumns { get; set; }

    /// <summary>Maximum number of word cloud terms.</summary>
    [Parameter]
    public int WordCloudMaximumTerms { get; set; }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var options = new ChartRenderOptions();
        if (Palette != null && Palette.Length > 0) options.Palette = Palette;
        if (IsBound(nameof(ShowLegend))) options.ShowLegend = ShowLegend.IsPresent;
        if (IsBound(nameof(NoLegend))) options.ShowLegend = !NoLegend.IsPresent;
        if (IsBound(nameof(ShowPointLegend))) options.ShowPointLegend = ShowPointLegend.IsPresent;
        if (IsBound(nameof(LegendPosition))) options.LegendPosition = LegendPosition;
        if (IsBound(nameof(NoAxes))) options.ShowAxes = !NoAxes.IsPresent;
        if (IsBound(nameof(NoCard))) options.ShowCard = !NoCard.IsPresent;
        if (IsBound(nameof(NoPlotBackground))) options.ShowPlotBackground = !NoPlotBackground.IsPresent;
        if (IsBound(nameof(Transparent))) options.TransparentBackground = Transparent.IsPresent;
        if (IsBound(nameof(ShowDataLabels))) options.ShowDataLabels = ShowDataLabels.IsPresent;
        if (IsBound(nameof(TickCount))) options.TickCount = TickCount;
        if (IsBound(nameof(HeatmapScale))) options.HeatmapScale = HeatmapScale;
        if (IsBound(nameof(NoHeatmapScale))) options.ShowHeatmapScale = !NoHeatmapScale.IsPresent;
        if (IsBound(nameof(PieLabelContent))) options.PieLabelContent = PieLabelContent;
        if (IsBound(nameof(DonutInnerRadiusRatio))) options.DonutInnerRadiusRatio = DonutInnerRadiusRatio;
        if (!string.IsNullOrWhiteSpace(DonutCenterValue)) options.DonutCenterValue = DonutCenterValue;
        if (!string.IsNullOrWhiteSpace(DonutCenterLabel)) options.DonutCenterLabel = DonutCenterLabel;
        if (IsBound(nameof(ProgressMaximum))) options.ProgressMaximum = ProgressMaximum;
        if (IsBound(nameof(NoProgressHandles))) options.ShowProgressHandles = !NoProgressHandles.IsPresent;
        if (IsBound(nameof(PictorialSymbol))) options.PictorialSymbol = PictorialSymbol;
        if (IsBound(nameof(PictorialColumns))) options.PictorialColumns = PictorialColumns;
        if (IsBound(nameof(WordCloudMaximumTerms))) options.WordCloudMaximumTerms = WordCloudMaximumTerms;
        WriteObject(options);
    }

    private bool IsBound(string parameterName) => MyInvocation.BoundParameters.ContainsKey(parameterName);
}
