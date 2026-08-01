using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Switches an ImagePlayground console story to a previously declared persistent tab.</summary>
/// <example>
///   <summary>Return to the initial PowerShell tab</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Select-ImageConsoleStoryTab -Id WindowsPowerShell</code>
///   <para>Activates the initial tab without clearing its existing transcript buffer.</para>
/// </example>
[Cmdlet(VerbsCommon.Select, "ImageConsoleStoryTab")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class SelectImageConsoleStoryTabCmdlet : PSCmdlet {
    /// <summary>Identifier of a tab declared earlier in the story. The initial tab is named main.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Duration of the visual switch to the selected tab.</summary>
    [Parameter]
    [ValidateRange(0, 2)]
    public double TransitionSeconds { get; set; } = 0.2;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.SelectTab,
            string.Empty,
            TerminalTextTone.Default,
            TransitionSeconds,
            null,
            Id));
    }
}
