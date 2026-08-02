using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Switches an ImagePlayground console story to a previously declared persistent tab.</summary>
/// <para>Selection is always an intentional timeline action. The target tab keeps its transcript, working directory, profile, and palette, so the story can pause on a ready session or continue writing exactly where that session stopped.</para>
/// <example>
///   <summary>Return to the initial PowerShell tab</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Select-ImageConsoleStoryTab -Id PowerShell
/// New-ImageConsoleStoryPause -Seconds 1.5
/// New-ImageConsoleStoryCommand -Text 'Get-ChildItem .\artifacts'</code>
///   <para>Returns to the retained PowerShell buffer, holds it in a ready state, then continues the same session.</para>
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
