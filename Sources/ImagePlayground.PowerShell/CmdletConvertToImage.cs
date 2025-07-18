using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Converts an image to a different format.</summary>
/// <para>Outputs a new file using the extension from <see cref="OutputPath"/>.</para>
/// <para>When <see cref="OutputPath"/> ends with <c>.ico</c>, the file is only copied if the source file is also an icon.</para>
/// <example>
///   <summary>Convert PNG to JPEG</summary>
///   <code>ConvertTo-Image -FilePath image.png -OutputPath image.jpg -Quality 85</code>
/// </example>
/// <example>
///   <summary>Convert JPEG to PNG</summary>
///   <code>ConvertTo-Image -FilePath photo.jpg -OutputPath photo.png -CompressionLevel 6</code>
/// </example>
[Cmdlet(VerbsData.ConvertTo, "Image")]
public sealed class ConvertToImageCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path including extension.</summary>
    /// <para>The extension determines the output format.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Quality for JPEG or WEBP images.</summary>
    [Parameter]
    public int? Quality { get; set; }

    /// <summary>Compression level for PNG images.</summary>
    [Parameter]
    public int? CompressionLevel { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"ConvertTo-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        var output = Helpers.ResolvePath(OutputPath);
        ImagePlayground.ImageHelper.ConvertTo(filePath, output, Quality, CompressionLevel);
    }
}