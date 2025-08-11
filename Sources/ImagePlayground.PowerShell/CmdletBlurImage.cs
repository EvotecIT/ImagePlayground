using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Applies Gaussian blur to an image.</summary>
/// <para>Use <see cref="Sigma"/> to adjust blur intensity.</para>
/// <example>
///   <summary>Blur with radius 5</summary>
///   <code>Blur-Image -FilePath in.png -OutputPath out.png -Sigma 5</code>
/// </example>
[Cmdlet("Blur", "Image")]
public sealed class BlurImageCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    /// <para>The image must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path.</summary>
    /// <para>Supported formats depend on the file extension.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Blur radius.</summary>
    /// <para>Optional strength for the blur effect.</para>
    [Parameter(Position = 2)]
    public float? Sigma { get; set; }

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Blur-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        if (Async.IsPresent) {
            ImagePlayground.ImageHelper.BlurAsync(filePath, output, Sigma).GetAwaiter().GetResult();
        } else {
            ImagePlayground.ImageHelper.Blur(filePath, output, Sigma);
        }
    }
}