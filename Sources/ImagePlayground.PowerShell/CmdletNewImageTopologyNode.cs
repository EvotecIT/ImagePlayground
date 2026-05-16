using ChartForgeX.Topology;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a topology node definition.</summary>
/// <para>Use this cmdlet inside <c>New-ImageTopology</c> to describe a node in a topology diagram.</para>
/// <example>
///   <summary>Create an API node</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopologyNode -Id api -Label 'API' -Kind Service -Status Healthy -Symbol API</code>
///   <para>Creates a ChartForgeX topology node that can be rendered by New-ImageTopology.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyNode")]
[OutputType(typeof(TopologyNode))]
public sealed class NewImageTopologyNodeCmdlet : PSCmdlet {
    /// <para>Stable node identifier used by topology edges.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <para>Node label rendered in the diagram.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string Label { get; set; } = string.Empty;

    /// <para>Optional node subtitle.</para>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <para>Node kind used for icon and legend selection.</para>
    [Parameter]
    public TopologyNodeKind Kind { get; set; } = TopologyNodeKind.Generic;

    /// <para>Node health or state.</para>
    [Parameter]
    public TopologyHealthStatus Status { get; set; } = TopologyHealthStatus.Unknown;

    /// <para>Optional parent group identifier.</para>
    [Parameter]
    public string GroupId { get; set; } = string.Empty;

    /// <para>Manual X coordinate. Automatic layouts can ignore this value.</para>
    [Parameter]
    public double X { get; set; }

    /// <para>Manual Y coordinate. Automatic layouts can ignore this value.</para>
    [Parameter]
    public double Y { get; set; }

    /// <para>Optional node longitude for geographic layouts.</para>
    [Parameter]
    public double Longitude { get; set; }

    /// <para>Optional node latitude for geographic layouts.</para>
    [Parameter]
    public double Latitude { get; set; }

    /// <para>Node width in pixels.</para>
    [Parameter]
    public double Width { get; set; } = 120;

    /// <para>Node height in pixels.</para>
    [Parameter]
    public double Height { get; set; } = 64;

    /// <para>Short symbol rendered inside or near the node icon.</para>
    [Parameter]
    public string Symbol { get; set; } = string.Empty;

    /// <para>Reusable ChartForgeX icon identifier.</para>
    [Parameter]
    public string IconId { get; set; } = string.Empty;

    /// <para>Optional node display mode override.</para>
    [Parameter]
    public TopologyNodeDisplayMode DisplayMode { get; set; } = TopologyNodeDisplayMode.Card;

    /// <para>Optional short badge text.</para>
    [Parameter]
    public string Badge { get; set; } = string.Empty;

    /// <para>Optional node accent color as a CSS hex color.</para>
    [Parameter]
    public string Color { get; set; } = string.Empty;

    /// <para>Optional node fill color as a CSS hex color.</para>
    [Parameter]
    public string BackgroundColor { get; set; } = string.Empty;

    /// <para>Optional drill-down link for SVG and HTML outputs.</para>
    [Parameter]
    public string Href { get; set; } = string.Empty;

    /// <para>Optional tooltip for SVG and HTML outputs.</para>
    [Parameter]
    public string Tooltip { get; set; } = string.Empty;

    /// <summary>Emits a topology node definition.</summary>
    protected override void ProcessRecord() {
        var node = new TopologyNode {
            Id = Id,
            Label = Label,
            Kind = Kind,
            Status = Status,
            X = X,
            Y = Y,
            Width = Width,
            Height = Height
        };

        if (!string.IsNullOrWhiteSpace(Subtitle)) {
            node.Subtitle = Subtitle;
        }
        if (!string.IsNullOrWhiteSpace(GroupId)) {
            node.GroupId = GroupId;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Longitude))) {
            node.Longitude = Longitude;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Latitude))) {
            node.Latitude = Latitude;
        }
        if (!string.IsNullOrWhiteSpace(Symbol)) {
            node.Symbol = Symbol;
        }
        if (!string.IsNullOrWhiteSpace(IconId)) {
            node.IconId = IconId;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(DisplayMode))) {
            node.DisplayMode = DisplayMode;
        }
        if (!string.IsNullOrWhiteSpace(Badge)) {
            node.Badge = Badge;
        }
        if (!string.IsNullOrWhiteSpace(Color)) {
            node.Color = Color;
        }
        if (!string.IsNullOrWhiteSpace(BackgroundColor)) {
            node.BackgroundColor = BackgroundColor;
        }
        if (!string.IsNullOrWhiteSpace(Href)) {
            node.Href = Href;
        }
        if (!string.IsNullOrWhiteSpace(Tooltip)) {
            node.Tooltip = Tooltip;
        }

        WriteObject(node);
    }
}
