using System.Management.Automation;
using ChartForgeX.Stories;
using ImagePlayground.PowerShell.Stories;

namespace ImagePlayground.PowerShell;

/// <summary>Converts exact source text into renderer-neutral syntax spans for visual stories.</summary>
/// <para>PowerShell uses the native System.Management.Automation parser. Other languages can provide an optional IStorySourceTokenizer adapter without adding parser dependencies to ChartForgeX.</para>
/// <example>
///   <summary>Tokenize PowerShell source</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$source = ConvertTo-ImageStorySource -Text 'Get-Process | Sort-Object CPU -Descending' -Language PowerShell</code>
///   <para>Returns exact source text with semantic spans suitable for New-ImageStoryPanel.</para>
/// </example>
[Cmdlet(VerbsData.ConvertTo, "ImageStorySource")]
[OutputType(typeof(StorySourceText))]
public sealed class ConvertToImageStorySourceCmdlet : PSCmdlet {
    /// <summary>Exact source text, including whitespace and line endings.</summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    [AllowEmptyString]
    public string Text { get; set; } = string.Empty;

    /// <summary>Language identifier. PowerShell uses its native parser; Plain preserves text without tokenization.</summary>
    [Parameter]
    public string Language { get; set; } = "PowerShell";

    /// <summary>Optional tokenizer adapter for C#, Bash, or another language.</summary>
    [Parameter]
    public IStorySourceTokenizer? Tokenizer { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Tokenizer != null) {
            WriteObject(Tokenizer.Tokenize(Text));
        } else if (Language.Equals("powershell", System.StringComparison.OrdinalIgnoreCase) ||
                   Language.Equals("pwsh", System.StringComparison.OrdinalIgnoreCase) ||
                   Language.Equals("ps1", System.StringComparison.OrdinalIgnoreCase)) {
            WriteObject(new PowerShellStorySourceTokenizer().Tokenize(Text));
        } else if (Language.Equals("plain", System.StringComparison.OrdinalIgnoreCase) ||
                   Language.Equals("text", System.StringComparison.OrdinalIgnoreCase)) {
            WriteObject(StorySourceText.Create(Text, Language));
        } else {
            var exception = new PSArgumentException("Language '" + Language + "' requires an IStorySourceTokenizer adapter. Use PowerShell, Plain, or pass -Tokenizer.");
            ThrowTerminatingError(new ErrorRecord(exception, "ImageStoryTokenizerRequired", ErrorCategory.NotInstalled, Language));
        }
    }
}
