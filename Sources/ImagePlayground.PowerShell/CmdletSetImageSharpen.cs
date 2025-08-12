using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Sharpens an image.</summary>
/// <para>Applies a Gaussian sharpen filter with the specified <see cref="Amount"/>.</para>
/// <example>
///   <summary>Sharpen an image</summary>
///   <code>Set-ImageSharpen -FilePath in.png -OutputPath out.png -Amount 2</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageSharpen")]
public sealed class SetImageSharpenCmdlet : PSCmdlet {
        /// <summary>Path to the source image.</summary>
        /// <para>The image must exist.</para>
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination file path.</summary>
        /// <para>Supported formats depend on the file extension.</para>
        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>Sharpen amount.</summary>
        /// <para>Specifies the strength of the sharpen filter.</para>
        [Parameter(Mandatory = true, Position = 2)]
        public float Amount { get; set; }

        /// <summary>Use asynchronous processing.</summary>
        [Parameter]
        public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageSharpen - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        Helpers.CreateParentDirectory(output);
        if (Async.IsPresent) {
            ImagePlayground.ImageHelper.SharpenAsync(filePath, output, Amount).GetAwaiter().GetResult();
        } else {
            ImagePlayground.ImageHelper.Sharpen(filePath, output, Amount);
        }
    }
}
