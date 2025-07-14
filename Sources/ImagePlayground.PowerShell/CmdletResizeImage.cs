using ImagePlayground;
namespace ImagePlayground.PowerShell;

/// <summary>Resizes an image.</summary>
/// <para>Use width/height parameters or <see cref="Percentage"/>.</para>
/// <example>
///   <summary>Resize to 100x100</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100</code>
/// </example>
/// <example>
///   <summary>Double the size</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Percentage 200</code>
/// </example>
[Cmdlet(VerbsCommon.Resize, "Image", DefaultParameterSetName = ParameterSetHeightWidth)]
public sealed class ResizeImageCmdlet : PSCmdlet {
        private const string ParameterSetHeightWidth = "HeightWidth";
        private const string ParameterSetPercentage = "Percentage";

        /// <summary>Path to the source image.</summary>
        /// <para>The image must exist.</para>
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetHeightWidth)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetPercentage)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination file path.</summary>
        /// <para>Supported formats depend on the file extension.</para>
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetHeightWidth)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetPercentage)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>New width of the image.</summary>
        /// <para>Used with <see cref="Height"/> when not using <see cref="Percentage"/>.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        [ValidateRange(1, 1000)]
        public int Width { get; set; }

        /// <summary>New height of the image.</summary>
        /// <para>Used with <see cref="Width"/> when not using <see cref="Percentage"/>.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        [ValidateRange(1, 1000)]
        public int Height { get; set; }

        /// <summary>Percentage based resize.</summary>
        /// <para>Applies uniform scaling relative to the original size.</para>
        [Parameter(ParameterSetName = ParameterSetPercentage)]
        [ValidateRange(1, 1000)]
        public int Percentage { get; set; }

        /// <summary>Ignore aspect ratio.</summary>
        /// <para>Only valid when resizing by width or height.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        public SwitchParameter DontRespectAspectRatio { get; set; }

        /// <summary>Use asynchronous processing.</summary>
        [Parameter]
        public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Resize-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);

        if (ParameterSetName == ParameterSetPercentage) {
            if (Async.IsPresent) {
                ImagePlayground.ImageHelper.ResizeAsync(filePath, output, Percentage).GetAwaiter().GetResult();
            } else {
                ImagePlayground.ImageHelper.Resize(filePath, output, Percentage);
            }
            return;
        }

        bool widthBound = MyInvocation.BoundParameters.ContainsKey(nameof(Width));
        bool heightBound = MyInvocation.BoundParameters.ContainsKey(nameof(Height));

        if (DontRespectAspectRatio.IsPresent && !widthBound && !heightBound) {
            var ex = new PSArgumentException("DontRespectAspectRatio requires Width or Height");
            ThrowTerminatingError(new ErrorRecord(ex, "MissingWidthOrHeight", ErrorCategory.InvalidArgument, null));
            return;
        }

        if (!widthBound && !heightBound) {
            WriteWarning("Resize-Image - Please specify Width or Height or Percentage.");
            return;
        }

        int? width = widthBound ? Width : (int?)null;
        int? height = heightBound ? Height : (int?)null;
        bool keepAspect = !DontRespectAspectRatio.IsPresent;

        if (Async.IsPresent) {
            ImagePlayground.ImageHelper.ResizeAsync(filePath, output, width, height, keepAspect).GetAwaiter().GetResult();
        } else {
            ImagePlayground.ImageHelper.Resize(filePath, output, width, height, keepAspect);
        }
    }
}
