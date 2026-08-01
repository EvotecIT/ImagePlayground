using System;
using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a typed command step for an ImagePlayground console story.</summary>
/// <example>
///   <summary>Show a PowerShell command</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryCommand -Text 'Get-Service -Name WinRM'</code>
///   <para>The command is presented but never executed.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryCommand")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryCommandCmdlet : PSCmdlet {
    /// <summary>Command text shown after the configured terminal prompt.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Text { get; set; } = string.Empty;

    /// <summary>Explicit typing duration in seconds, or zero to use automatic timing.</summary>
    [Parameter]
    [ValidateRange(0, 20)]
    public double DurationSeconds { get; set; }

    /// <inheritdoc />
    protected override void EndProcessing() {
        if (DurationSeconds > 0 && DurationSeconds < 0.05) {
            var exception = new PSArgumentOutOfRangeException(nameof(DurationSeconds), DurationSeconds, "Command duration must be zero or between 0.05 and 20 seconds.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryCommandInvalidDuration", ErrorCategory.InvalidArgument, DurationSeconds));
        }

        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.Command,
            Text,
            TerminalTextTone.Default,
            DurationSeconds,
            null));
    }
}
