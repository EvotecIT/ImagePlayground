using ChartForgeX.Topology;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an ordered topology scenario.</summary>
/// <para>Scenarios drive HTML route controls, highlighted static views, and script-free SVG, GIF, and APNG route motion.</para>
/// <example>
///   <summary>Create a request-flow scenario</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopologyScenario -Id request -Label 'Request flow' -StepDefinition { New-ImageTopologyScenarioStep -Id client-api -Kind Edge; New-ImageTopologyScenarioStep -Id api-db -Kind Edge } -AutoPlay</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyScenario")]
[OutputType(typeof(TopologyScenario))]
public sealed class NewImageTopologyScenarioCmdlet : PSCmdlet {
    /// <summary>Stable scenario identifier.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Scenario label shown by HTML controls.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Label { get; set; } = string.Empty;

    /// <summary>Scenario steps supplied directly.</summary>
    [Parameter]
    public TopologyScenarioStep[] Step { get; set; } = Array.Empty<TopologyScenarioStep>();

    /// <summary>Script block that emits steps from <c>New-ImageTopologyScenarioStep</c>.</summary>
    [Parameter]
    public ScriptBlock? StepDefinition { get; set; }

    /// <summary>Optional scenario description.</summary>
    [Parameter]
    public string Description { get; set; } = string.Empty;

    /// <summary>Optional CSS color used to accent the scenario.</summary>
    [Parameter]
    public string Color { get; set; } = string.Empty;

    /// <summary>Default duration of each step in milliseconds.</summary>
    [Parameter]
    [ValidateRange(200, 60000)]
    public int StepDurationMilliseconds { get; set; } = 900;

    /// <summary>Loop scenario playback.</summary>
    [Parameter]
    public SwitchParameter Loop { get; set; }

    /// <summary>Allow interactive HTML output to begin playback automatically.</summary>
    [Parameter]
    public SwitchParameter AutoPlay { get; set; }

    /// <summary>Dim topology elements that do not participate in the scenario.</summary>
    [Parameter]
    public SwitchParameter Spotlight { get; set; }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var steps = new List<TopologyScenarioStep>(Step);
        if (StepDefinition != null) {
            foreach (var result in StepDefinition.Invoke()) {
                var value = result is PSObject psObject ? psObject.BaseObject : result;
                if (value is TopologyScenarioStep step) {
                    steps.Add(step);
                } else if (value != null) {
                    var exception = new PSArgumentException("StepDefinition must emit only New-ImageTopologyScenarioStep results.");
                    ThrowTerminatingError(new ErrorRecord(exception, "NewImageTopologyScenarioUnsupportedStep", ErrorCategory.InvalidArgument, value));
                }
            }
        }

        if (steps.Count == 0) {
            var exception = new PSArgumentException("A topology scenario requires at least one node or edge step.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageTopologyScenarioMissingStep", ErrorCategory.InvalidArgument, null));
        }

        var scenario = new TopologyScenario {
            Id = Id,
            Label = Label,
            PlaybackDelayMilliseconds = StepDurationMilliseconds,
            LoopPlayback = Loop.IsPresent,
            AutoPlay = AutoPlay.IsPresent,
            Spotlight = Spotlight.IsPresent
        };
        if (!string.IsNullOrWhiteSpace(Description)) scenario.Description = Description;
        if (!string.IsNullOrWhiteSpace(Color)) scenario.Color = Color;
        scenario.Steps.AddRange(steps);
        WriteObject(scenario);
    }
}
