using ChartForgeX.Primitives;
using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a dashboard metric card.</summary>
/// <para>The returned ChartForgeX block can be composed by <c>New-ImageVisualGrid</c> or <c>New-ImageVisualStory</c>.</para>
/// <example>
///   <summary>Create a metric card with a sparkline</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageMetricCard -Label 'Requests' -Value 12840 -Trend '+12%' -Status Positive -MiniValues 8200,9400,10100,12840</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageMetricCard")]
[OutputType(typeof(MetricCard))]
public sealed class NewImageMetricCardCmdlet : PSCmdlet {
    /// <summary>Metric label.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Label { get; set; } = string.Empty;

    /// <summary>Metric value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public object? Value { get; set; }

    /// <summary>Optional numeric or date format string.</summary>
    [Parameter]
    public string Format { get; set; } = string.Empty;

    /// <summary>Optional unit displayed beside the value.</summary>
    [Parameter]
    public string Unit { get; set; } = string.Empty;

    /// <summary>Optional supporting caption.</summary>
    [Parameter]
    public string Caption { get; set; } = string.Empty;

    /// <summary>Optional trend text.</summary>
    [Parameter]
    public string Trend { get; set; } = string.Empty;

    /// <summary>Semantic metric status.</summary>
    [Parameter]
    public VisualStatus Status { get; set; } = VisualStatus.Neutral;

    /// <summary>Built-in metric icon.</summary>
    [Parameter]
    public VisualIcon Icon { get; set; } = VisualIcon.None;

    /// <summary>Values used by the optional mini chart.</summary>
    [Parameter]
    public double[] MiniValues { get; set; } = Array.Empty<double>();

    /// <summary>Mini chart kind.</summary>
    [Parameter]
    [ValidateSet("Sparkline", "Bars")]
    public string MiniChart { get; set; } = "Sparkline";

    /// <summary>Optional mini chart color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? MiniColor { get; set; }

    /// <summary>Card width in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 320;

    /// <summary>Card height in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 176;

    /// <summary>ChartForgeX theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <summary>Hide the card surface.</summary>
    [Parameter]
    public SwitchParameter NoCard { get; set; }

    /// <summary>Use a transparent card background.</summary>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var card = MetricCard.Create()
            .WithMetric(Label, Value, string.IsNullOrWhiteSpace(Format) ? null : Format, string.IsNullOrWhiteSpace(Unit) ? null : Unit)
            .WithSize(Width, Height)
            .WithTheme(ChartThemeResolver.Resolve(Theme))
            .WithStatus(Status)
            .WithIcon(Icon)
            .WithCard(!NoCard.IsPresent)
            .WithTransparentBackground(Transparent.IsPresent);
        if (!string.IsNullOrWhiteSpace(Caption)) card.WithCaption(Caption);
        if (!string.IsNullOrWhiteSpace(Trend)) card.WithTrend(Trend);
        if (MiniValues.Length > 0) {
            var color = ChartColorConverter.Convert(MiniColor);
            if (MiniChart.Equals("Bars", StringComparison.OrdinalIgnoreCase)) card.WithMiniBars(MiniValues, color: color);
            else card.WithMiniSparkline(MiniValues, color: color);
        }
        WriteObject(card);
    }
}
