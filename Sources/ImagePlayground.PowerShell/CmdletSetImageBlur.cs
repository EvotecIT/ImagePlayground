using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Blurs an image.</summary>
/// <para>Applies a Gaussian blur with the specified <see cref="Amount"/>.</para>
/// <example>
///   <summary>Blur an image</summary>
///   <code>Set-ImageBlur -FilePath in.png -OutputPath out.png -Amount 5</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageBlur")]
public sealed class SetImageBlurCmdlet : PSCmdlet {
        /// <summary>Path to the source image.</summary>
        /// <para>The image must exist.</para>
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination file path.</summary>
        /// <para>Supported formats depend on the file extension.</para>
        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>Blur amount.</summary>
        /// <para>Specifies the strength of the blur.</para>
        [Parameter(Mandatory = true, Position = 2)]
        public float Amount { get; set; }

        /// <summary>Use asynchronous processing.</summary>
        [Parameter]
        public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageBlur - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        if (Async.IsPresent) {
            ImagePlayground.ImageHelper.BlurAsync(filePath, output, Amount).GetAwaiter().GetResult();
        } else {
            ImagePlayground.ImageHelper.Blur(filePath, output, Amount);
        }
    }
}
