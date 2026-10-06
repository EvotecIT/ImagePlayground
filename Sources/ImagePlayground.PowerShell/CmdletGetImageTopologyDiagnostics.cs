using System.Management.Automation;
using ChartForgeX.Topology;

namespace ImagePlayground.PowerShell;

/// <summary>Analyzes prepared ChartForgeX topology geometry and routing.</summary>
/// <para>Returns machine-readable node bounds, named port positions, edge routes, fallback reasons, overlap scores, and collisions.</para>
/// <example>
///   <summary>Inspect the layout of a service diagram</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$chart = New-ImageTopology -TopologyDefinition {
///     New-ImageTopologyNode -Id api -Label API -Kind Service
///     New-ImageTopologyNode -Id db -Label Database -Kind Database
///     New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -Kind Dependency
/// } -Layout Layered -FilePath service-map.svg -PassThru
/// $chart | Get-ImageTopologyDiagnostics -LayoutPreset Balanced</code>
///   <para>Exports a service diagram and returns its node and edge layout diagnostics.</para>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageTopologyDiagnostics")]
[OutputType(typeof(TopologyLayoutDiagnosticReport))]
public sealed class GetImageTopologyDiagnosticsCmdlet : PSCmdlet {
    /// <para>Topology chart to analyze.</para>
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public TopologyChart? Topology { get; set; }

    /// <para>Reusable topology spacing and presentation profile.</para>
    [Parameter]
    public TopologyLayoutPreset LayoutPreset { get; set; } = TopologyLayoutPreset.Automatic;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Topology == null) {
            throw new PSArgumentNullException(nameof(Topology));
        }
        WriteObject(TopologyLayoutDiagnostics.Analyze(Topology, new TopologyRenderOptions { LayoutPreset = LayoutPreset }));
    }
}
