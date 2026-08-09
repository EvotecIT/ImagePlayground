using System.Management.Automation;
using ChartForgeX.Topology;

namespace ImagePlayground.PowerShell;

/// <summary>Analyzes prepared ChartForgeX topology geometry and routing.</summary>
/// <para>Returns machine-readable node bounds, named port positions, edge routes, fallback reasons, overlap scores, and collisions.</para>
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
