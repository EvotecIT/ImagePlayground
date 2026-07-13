using ChartForgeX.Topology;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a topology group definition.</summary>
/// <para>Use this cmdlet inside <c>New-ImageTopology</c> to describe a logical region or cluster.</para>
/// <example>
///   <summary>Create a site group</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopologyGroup -Id site-a -Label 'Site A' -Status Healthy -Symbol region</code>
///   <para>Creates a ChartForgeX topology group that can contain topology nodes.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyGroup")]
[OutputType(typeof(TopologyGroup))]
public sealed class NewImageTopologyGroupCmdlet : PSCmdlet {
    /// <para>Stable group identifier.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <para>Group label rendered in the diagram.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string Label { get; set; } = string.Empty;

    /// <para>Optional group subtitle.</para>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <para>Group health or state.</para>
    [Parameter]
    public TopologyHealthStatus Status { get; set; } = TopologyHealthStatus.Unknown;

    /// <para>Manual X coordinate. Automatic layouts can ignore this value.</para>
    [Parameter]
    public double X { get; set; }

    /// <para>Manual Y coordinate. Automatic layouts can ignore this value.</para>
    [Parameter]
    public double Y { get; set; }

    /// <para>Group width in pixels.</para>
    [Parameter]
    public double Width { get; set; } = 320;

    /// <para>Group height in pixels.</para>
    [Parameter]
    public double Height { get; set; } = 220;

    /// <para>Optional group longitude for geographic layouts.</para>
    [Parameter]
    public double Longitude { get; set; }

    /// <para>Optional group latitude for geographic layouts.</para>
    [Parameter]
    public double Latitude { get; set; }

    /// <para>Preferred node arrangement policy for dense grouped layouts.</para>
    [Parameter]
    public TopologyGroupLayoutPolicy LayoutPolicy { get; set; } = TopologyGroupLayoutPolicy.Auto;

    /// <para>Short symbol rendered near the group header.</para>
    [Parameter]
    public string Symbol { get; set; } = string.Empty;

    /// <para>Reusable ChartForgeX icon identifier.</para>
    [Parameter]
    public string IconId { get; set; } = string.Empty;

    /// <para>Optional group accent color as a CSS hex color.</para>
    [Parameter]
    public string Color { get; set; } = string.Empty;

    /// <para>Optional drill-down link for SVG and HTML outputs.</para>
    [Parameter]
    public string Href { get; set; } = string.Empty;

    /// <para>Optional tooltip for SVG and HTML outputs.</para>
    [Parameter]
    public string Tooltip { get; set; } = string.Empty;

    /// <summary>Emits a topology group definition.</summary>
    protected override void ProcessRecord() {
        var group = new TopologyGroup {
            Id = Id,
            Label = Label,
            Status = Status,
            X = X,
            Y = Y,
            Width = Width,
            Height = Height,
            LayoutPolicy = LayoutPolicy
        };

        if (!string.IsNullOrWhiteSpace(Subtitle)) {
            group.Subtitle = Subtitle;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Longitude))) {
            group.Longitude = Longitude;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Latitude))) {
            group.Latitude = Latitude;
        }
        if (!string.IsNullOrWhiteSpace(Symbol)) {
            group.Symbol = Symbol;
        }
        if (!string.IsNullOrWhiteSpace(IconId)) {
            group.IconId = IconId;
        }
        if (!string.IsNullOrWhiteSpace(Color)) {
            group.Color = Color;
        }
        if (!string.IsNullOrWhiteSpace(Href)) {
            group.Href = Href;
        }
        if (!string.IsNullOrWhiteSpace(Tooltip)) {
            group.Tooltip = Tooltip;
        }

        WriteObject(group);
    }
}
