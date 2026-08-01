using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a blank-line step for an ImagePlayground console story.</summary>
/// <example>
///   <summary>Separate two commands</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryBlankLine</code>
///   <para>Adds one blank terminal line.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryBlankLine")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryBlankLineCmdlet : PSCmdlet {
    /// <inheritdoc />
    protected override void EndProcessing() {
        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.Blank,
            string.Empty,
            TerminalTextTone.Default,
            0,
            null));
    }
}
