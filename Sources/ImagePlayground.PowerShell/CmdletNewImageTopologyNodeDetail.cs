using System.Management.Automation;
using ChartForgeX.Topology;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a typed label/value detail row for a topology node card.</summary>
[Cmdlet(VerbsCommon.New, "ImageTopologyNodeDetail")]
[OutputType(typeof(TopologyNodeDetail))]
public sealed class NewImageTopologyNodeDetailCmdlet : PSCmdlet {
    /// <para>Detail label.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string Label { get; set; } = string.Empty;

    /// <para>Detail value.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string Value { get; set; } = string.Empty;

    /// <para>Optional semantic status.</para>
    [Parameter]
    public TopologyHealthStatus? Status { get; set; }

    /// <para>Optional row accent color.</para>
    [Parameter]
    public string Color { get; set; } = string.Empty;

    /// <para>Optional reusable icon identifier.</para>
    [Parameter]
    public string IconId { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new TopologyNodeDetail {
            Label = Label,
            Value = Value,
            Status = Status,
            Color = string.IsNullOrWhiteSpace(Color) ? null : Color,
            IconId = string.IsNullOrWhiteSpace(IconId) ? null : IconId
        });
    }
}
