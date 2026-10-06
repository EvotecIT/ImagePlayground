using System.Management.Automation;
using ChartForgeX.Topology;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a named attachment port for a topology node.</summary>
/// <example>
///   <summary>Add an output port to a service node</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$port = New-ImageTopologyNodePort -Id output -Side Right -Offset 0.5 -Label Database
/// New-ImageTopologyNode -Id api -Label API -Kind Service -Port $port</code>
///   <para>Creates a port halfway along the right boundary and attaches it to the service node.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyNodePort")]
[OutputType(typeof(TopologyNodePort))]
public sealed class NewImageTopologyNodePortCmdlet : PSCmdlet {
    /// <para>Node-local stable port identifier.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <para>Explicit node boundary side.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public TopologyEdgePort Side { get; set; }

    /// <para>Normalized position along the side from zero to one.</para>
    [Parameter]
    [ValidateRange(0D, 1D)]
    public double Offset { get; set; } = 0.5D;

    /// <para>Optional visible or host-facing label.</para>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new TopologyNodePort {
            Id = Id,
            Side = Side,
            Offset = Offset,
            Label = string.IsNullOrWhiteSpace(Label) ? null : Label
        });
    }
}
