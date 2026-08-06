using ChartForgeX.Topology;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one node or edge step for a topology scenario.</summary>
/// <para>Use the result inside <c>New-ImageTopologyScenario</c> to define an ordered route.</para>
/// <example>
///   <summary>Create an edge route step</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopologyScenarioStep -Id api-db -Kind Edge -Label 'Database call' -DurationMilliseconds 700</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyScenarioStep")]
[OutputType(typeof(TopologyScenarioStep))]
public sealed class NewImageTopologyScenarioStepCmdlet : PSCmdlet {
    /// <summary>Referenced topology node or edge identifier.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Whether the step references a node or edge.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public TopologyScenarioStepKind Kind { get; set; }

    /// <summary>Optional step label.</summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>Optional step description.</summary>
    [Parameter]
    public string Description { get; set; } = string.Empty;

    /// <summary>Optional playback duration override in milliseconds.</summary>
    [Parameter]
    [ValidateRange(200, 60000)]
    public int DurationMilliseconds { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var step = new TopologyScenarioStep {
            Id = Id,
            Kind = Kind
        };
        if (!string.IsNullOrWhiteSpace(Label)) step.Label = Label;
        if (!string.IsNullOrWhiteSpace(Description)) step.Description = Description;
        if (MyInvocation.BoundParameters.ContainsKey(nameof(DurationMilliseconds))) step.DurationMilliseconds = DurationMilliseconds;
        WriteObject(step);
    }
}
