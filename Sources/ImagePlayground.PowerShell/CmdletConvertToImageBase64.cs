using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Converts an image file into a Base64 encoded string.
/// </summary>
/// <example>
///   <summary>Get a Base64 string from an image</summary>
///   <code>ConvertTo-ImageBase64 -FilePath in.png</code>
/// </example>
[Cmdlet(VerbsData.ConvertTo, "ImageBase64")]
[OutputType(typeof(string))]
public sealed class ConvertToImageBase64Cmdlet : PSCmdlet {
    /// <summary>Path to the image to convert.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"ConvertTo-ImageBase64 - File {FilePath} not found. Please check the path.");
            return;
        }

        var result = ImageHelper.ConvertToBase64(filePath);
        WriteObject(result);
    }
}
