using System.Management.Automation;
using ChartForgeX.Motion;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one named motion cue for an animated ChartForgeX visual story.</summary>
/// <para>Use this cmdlet inside the MotionDefinition block of New-ImageVisualStory. Each cue targets the stable id assigned to a visual-grid panel or the built-in title and subtitle targets.</para>
/// <example>
///   <summary>Reveal the story title</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.9</code>
///   <para>Creates a restrained left-to-right title reveal with the default emphasized easing.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageVisualMotionCue")]
[OutputType(typeof(VisualMotionCue))]
public sealed class NewImageVisualMotionCueCmdlet : PSCmdlet {
    /// <summary>Stable panel id, or the built-in title or subtitle target.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string TargetId { get; set; } = string.Empty;

    /// <summary>Entrance or emphasis effect.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public VisualMotionEffect Effect { get; set; }

    /// <summary>Delay before the cue starts, in seconds.</summary>
    [Parameter]
    [ValidateRange(0, 60)]
    public double DelaySeconds { get; set; }

    /// <summary>Cue duration, in seconds.</summary>
    [Parameter]
    [ValidateRange(0.01, 60)]
    public double DurationSeconds { get; set; } = 0.7;

    /// <summary>Timing curve used by the cue.</summary>
    [Parameter]
    public VisualMotionEasing Easing { get; set; } = VisualMotionEasing.Emphasized;

    /// <summary>Travel distance used by positional effects, in pixels.</summary>
    [Parameter]
    [ValidateRange(0, 80)]
    public double DistancePixels { get; set; } = 12;

    /// <inheritdoc />
    protected override void EndProcessing() {
        WriteObject(new VisualMotionCue(TargetId, Effect)
            .WithTiming(DelaySeconds, DurationSeconds)
            .WithEasing(Easing)
            .WithDistance(DistancePixels));
    }
}
