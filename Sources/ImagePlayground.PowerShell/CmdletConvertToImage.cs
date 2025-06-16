using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Converts an image to a different format.</summary>
/// <para>Outputs a new file using the extension from <see cref="OutputPath"/>.</para>
/// <example>
///   <summary>Convert PNG to JPEG</summary>
///   <code>ConvertTo-Image -FilePath image.png -OutputPath image.jpg</code>
/// </example>
[Cmdlet(VerbsData.ConvertTo, "Image")]
public sealed class ConvertToImageCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path including extension.</summary>
    /// <para>The extension determines the output format.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"ConvertTo-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        ImagePlayground.ImageHelper.ConvertTo(FilePath, OutputPath);
    }
}