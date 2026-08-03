using System.Management.Automation;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a typed output step for an ImagePlayground console story.</summary>
/// <example>
///   <summary>Show successful script output</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success</code>
///   <para>The semantic tone is rendered consistently across SVG, HTML, PNG, GIF, and APNG output.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryOutput")]
[OutputType(typeof(ImageConsoleStoryStep))]
public sealed class NewImageConsoleStoryOutputCmdlet : PSCmdlet {
    /// <summary>One or more lines of terminal output.</summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    [AllowEmptyString]
    public string Text { get; set; } = string.Empty;

    /// <summary>Semantic output color.</summary>
    [Parameter]
    [Alias("Tone")]
    public TerminalTextTone Style { get; set; } = TerminalTextTone.Default;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImageConsoleStoryStep(
            TerminalStoryStepKind.Output,
            Text,
            Style,
            0,
            null));
    }
}
