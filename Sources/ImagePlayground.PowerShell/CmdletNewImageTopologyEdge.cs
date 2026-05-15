using ChartForgeX.Topology;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a topology edge definition.</summary>
/// <para>Use this cmdlet inside <c>New-ImageTopology</c> to describe a relationship between topology nodes.</para>
/// <example>
///   <summary>Create a dependency edge</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopologyEdge -SourceNodeId api -TargetNodeId database -Label '32 ms' -Kind Dependency -Status Warning -Direction Forward</code>
///   <para>Creates a ChartForgeX topology edge that can be rendered by New-ImageTopology.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyEdge")]
[OutputType(typeof(TopologyEdge))]
public sealed class NewImageTopologyEdgeCmdlet : PSCmdlet {
    /// <para>Stable edge identifier. When omitted, one is derived from source and target node ids.</para>
    [Parameter]
    public string Id { get; set; } = string.Empty;

    /// <para>Source node identifier.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string SourceNodeId { get; set; } = string.Empty;

    /// <para>Target node identifier.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string TargetNodeId { get; set; } = string.Empty;

    /// <para>Primary edge label.</para>
    [Parameter(Position = 2)]
    public string Label { get; set; } = string.Empty;

    /// <para>Secondary edge label.</para>
    [Parameter]
    public string SecondaryLabel { get; set; } = string.Empty;

    /// <para>Tertiary edge label.</para>
    [Parameter]
    public string TertiaryLabel { get; set; } = string.Empty;

    /// <para>Relationship kind.</para>
    [Parameter]
    public TopologyEdgeKind Kind { get; set; } = TopologyEdgeKind.Generic;

    /// <para>Relationship health or state.</para>
    [Parameter]
    public TopologyHealthStatus Status { get; set; } = TopologyHealthStatus.Unknown;

    /// <para>Direction marker behavior.</para>
    [Parameter]
    public TopologyDirection Direction { get; set; } = TopologyDirection.None;

    /// <para>Edge routing mode.</para>
    [Parameter]
    public TopologyEdgeRouting Routing { get; set; } = TopologyEdgeRouting.Orthogonal;

    /// <para>Edge line style.</para>
    [Parameter]
    public TopologyEdgeLineStyle LineStyle { get; set; } = TopologyEdgeLineStyle.Auto;

    /// <para>Edge visual emphasis.</para>
    [Parameter]
    public TopologyEdgeEmphasis Emphasis { get; set; } = TopologyEdgeEmphasis.Normal;

    /// <para>Preferred source attachment side.</para>
    [Parameter]
    public TopologyEdgePort SourcePort { get; set; } = TopologyEdgePort.Auto;

    /// <para>Preferred target attachment side.</para>
    [Parameter]
    public TopologyEdgePort TargetPort { get; set; } = TopologyEdgePort.Auto;

    /// <para>Optional route lane offset in pixels.</para>
    [Parameter]
    public double RouteLane { get; set; }

    /// <para>Optional horizontal edge-label offset in pixels.</para>
    [Parameter]
    public double LabelOffsetX { get; set; }

    /// <para>Optional vertical edge-label offset in pixels.</para>
    [Parameter]
    public double LabelOffsetY { get; set; }

    /// <para>Optional edge color as a CSS hex color.</para>
    [Parameter]
    public string Color { get; set; } = string.Empty;

    /// <para>Render the edge as a quiet structural relationship.</para>
    [Parameter]
    public SwitchParameter Muted { get; set; }

    /// <para>Optional drill-down link for SVG and HTML outputs.</para>
    [Parameter]
    public string Href { get; set; } = string.Empty;

    /// <para>Optional tooltip for SVG and HTML outputs.</para>
    [Parameter]
    public string Tooltip { get; set; } = string.Empty;

    /// <summary>Emits a topology edge definition.</summary>
    protected override void ProcessRecord() {
        var edge = new TopologyEdge {
            Id = string.IsNullOrWhiteSpace(Id) ? SourceNodeId + "-" + TargetNodeId : Id,
            SourceNodeId = SourceNodeId,
            TargetNodeId = TargetNodeId,
            Kind = Kind,
            Status = Status,
            Direction = Direction,
            Routing = Routing,
            LineStyle = LineStyle,
            Emphasis = Emphasis,
            SourcePort = SourcePort,
            TargetPort = TargetPort,
            IsMuted = Muted.IsPresent
        };

        if (!string.IsNullOrWhiteSpace(Label)) {
            edge.Label = Label;
        }
        if (!string.IsNullOrWhiteSpace(SecondaryLabel)) {
            edge.SecondaryLabel = SecondaryLabel;
        }
        if (!string.IsNullOrWhiteSpace(TertiaryLabel)) {
            edge.TertiaryLabel = TertiaryLabel;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(RouteLane))) {
            edge.RouteLane = RouteLane;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(LabelOffsetX))) {
            edge.LabelOffsetX = LabelOffsetX;
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(LabelOffsetY))) {
            edge.LabelOffsetY = LabelOffsetY;
        }
        if (!string.IsNullOrWhiteSpace(Color)) {
            edge.Color = Color;
        }
        if (!string.IsNullOrWhiteSpace(Href)) {
            edge.Href = Href;
        }
        if (!string.IsNullOrWhiteSpace(Tooltip)) {
            edge.Tooltip = Tooltip;
        }

        WriteObject(edge);
    }
}
