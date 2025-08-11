using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Sharpens an image using Gaussian algorithm.</summary>
/// <para>Use <see cref="Sigma"/> to adjust sharpen intensity.</para>
/// <example>
///   <summary>Sharpen with radius 5</summary>
///   <code>Sharpen-Image -FilePath in.png -OutputPath out.png -Sigma 5</code>
/// </example>
[Cmdlet("Sharpen", "Image")]
public sealed class SharpenImageCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    /// <para>The image must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path.</summary>
    /// <para>Supported formats depend on the file extension.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Sharpen strength.</summary>
    /// <para>Optional strength for the sharpen effect.</para>
    [Parameter(Position = 2)]
    public float? Sigma { get; set; }

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Sharpen-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        if (Async.IsPresent) {
            ImagePlayground.ImageHelper.SharpenAsync(filePath, output, Sigma).GetAwaiter().GetResult();
        } else {
            ImagePlayground.ImageHelper.Sharpen(filePath, output, Sigma);
        }
    }
}