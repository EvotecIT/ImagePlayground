using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a silent timeline pause for an ImagePlayground console story.</summary>
/// <example>
///   <summary>Hold before showing the next command</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryPause -Seconds 0.8</code>
///   <para>The pause affects animated SVG, HTML, GIF, and APNG timelines.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryPause")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryPauseCmdlet : PSCmdlet {
    /// <summary>Pause duration in seconds.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    [ValidateRange(0.01, 10)]
    public double Seconds { get; set; }

    /// <inheritdoc />
    protected override void EndProcessing() {
        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.Pause,
            string.Empty,
            TerminalTextTone.Default,
            Seconds,
            null));
    }
}
