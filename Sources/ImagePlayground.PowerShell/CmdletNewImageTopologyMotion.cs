using ChartForgeX.Topology;
using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates script-free route motion options for topology output.</summary>
/// <para>Use a scenario id, explicit edge ids, or neither to animate the active or first scenario.</para>
/// <example>
///   <summary>Animate a named route</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$motion = New-ImageTopologyMotion -ScenarioId request -DurationSeconds 4 -FramesPerSecond 12</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopologyMotion")]
[OutputType(typeof(TopologyMotionOptions))]
public sealed class NewImageTopologyMotionCmdlet : PSCmdlet {
    /// <summary>Optional scenario identifier used as the motion route.</summary>
    [Parameter]
    public string ScenarioId { get; set; } = string.Empty;

    /// <summary>Optional explicit edge identifiers used as the motion route.</summary>
    [Parameter]
    public string[] EdgeId { get; set; } = Array.Empty<string>();

    /// <summary>Animation duration in seconds.</summary>
    [Parameter]
    [ValidateRange(0.01, 3600)]
    public double DurationSeconds { get; set; } = 4;

    /// <summary>Frame rate sampled for GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(0.1, 120)]
    public double FramesPerSecond { get; set; } = 12;

    /// <summary>Maximum sampled raster frame count.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int MaximumRasterFrames { get; set; } = 240;

    /// <summary>Moving route marker radius.</summary>
    [Parameter]
    [ValidateRange(0.1, 100)]
    public double MarkerRadius { get; set; } = 5.5;

    /// <summary>Optional marker color override.</summary>
    [Parameter]
    public string MarkerColor { get; set; } = string.Empty;

    /// <summary>Render one animation cycle instead of looping.</summary>
    [Parameter]
    public SwitchParameter NoLoop { get; set; }

    /// <summary>Disable endpoint node pulses for explicit-edge routes.</summary>
    [Parameter]
    public SwitchParameter NoEndpointPulses { get; set; }

    /// <summary>Optional static progress position from zero to one.</summary>
    [Parameter]
    [ValidateRange(0, 1)]
    public double Progress { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var motion = TopologyMotionOptions.RoutePulse(string.IsNullOrWhiteSpace(ScenarioId) ? null : ScenarioId, EdgeId);
        motion.WithDuration(DurationSeconds)
            .WithFrameRate(FramesPerSecond)
            .WithFrameLimit(MaximumRasterFrames)
            .WithMarker(MarkerRadius, string.IsNullOrWhiteSpace(MarkerColor) ? null : MarkerColor)
            .WithEndpointPulses(!NoEndpointPulses.IsPresent);
        motion.Loop = !NoLoop.IsPresent;
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Progress))) motion.AtProgress(Progress);
        WriteObject(motion);
    }
}
