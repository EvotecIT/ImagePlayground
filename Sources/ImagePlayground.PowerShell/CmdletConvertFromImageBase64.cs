using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Converts a Base64 encoded string into an image file.
/// </summary>
/// <example>
///   <summary>Save the Base64 content to a PNG file</summary>
///   <code>ConvertFrom-ImageBase64 -Base64 $content -OutputPath out.png</code>
/// </example>
[Cmdlet(VerbsData.ConvertFrom, "ImageBase64")]
public sealed class ConvertFromImageBase64Cmdlet : PSCmdlet {
    /// <summary>Base64 encoded image data.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Base64 { get; set; } = string.Empty;

    /// <summary>Path where the image will be written.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Open the newly created file after saving.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    protected override void ProcessRecord() {
        var output = Helpers.ResolvePath(OutputPath);
        ImageHelper.ConvertFromBase64(Base64, output);
        Helpers.Open(output, Open.IsPresent);
    }
}
