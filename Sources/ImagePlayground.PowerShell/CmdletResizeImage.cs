using ImagePlayground;
namespace ImagePlayground.PowerShell;

/// <summary>Resizes an image.</summary>
/// <para>Width and height bound the resized image while preserving its aspect ratio. Use <see cref="DontRespectAspectRatio"/> to stretch to the supplied dimensions, or <see cref="Percentage"/> for uniform scaling.</para>
/// <example>
///   <summary>Fit inside a 100x100 box</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100</code>
/// </example>
/// <example>
///   <summary>Stretch to 100x100</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100 -DontRespectAspectRatio</code>
/// </example>
/// <example>
///   <summary>Double the size</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Percentage 200</code>
/// </example>
[Cmdlet(VerbsCommon.Resize, "Image", DefaultParameterSetName = ParameterSetHeightWidth)]
public sealed class ResizeImageCmdlet : AsyncImageCmdlet {
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

        /// <summary>Requested width or maximum width when both bounds are supplied.</summary>
        /// <para>When aspect ratio is preserved, a width alone determines the corresponding height.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        [ValidateRange(1, 1000)]
        public int Width { get; set; }

        /// <summary>Requested height or maximum height when both bounds are supplied.</summary>
        /// <para>When aspect ratio is preserved, a height alone determines the corresponding width.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        [ValidateRange(1, 1000)]
        public int Height { get; set; }

        /// <summary>Percentage based resize.</summary>
        /// <para>Applies uniform scaling relative to the original size.</para>
        [Parameter(ParameterSetName = ParameterSetPercentage)]
        [ValidateRange(1, 1000)]
        public int Percentage { get; set; }

        /// <summary>Stretch to the supplied dimensions.</summary>
        /// <para>Disables aspect ratio preservation. An omitted dimension retains its original value.</para>
        [Parameter(ParameterSetName = ParameterSetHeightWidth)]
        public SwitchParameter DontRespectAspectRatio { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var filePath = ResolveExistingFilePath(FilePath, "ResizeImageFileNotFound", FilePath);
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, OutputPath);

        if (ParameterSetName == ParameterSetPercentage) {
            await ImagePlayground.ImageHelper.ResizeAsync(filePath, output, Percentage, CancelToken).ConfigureAwait(false);
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
            var exception = new PSArgumentException("Width or Height or Percentage must be specified.");
            ThrowTerminatingError(new ErrorRecord(exception, "ResizeImageMissingDimensions", ErrorCategory.InvalidArgument, null));
            return;
        }

        int? width = widthBound ? Width : (int?)null;
        int? height = heightBound ? Height : (int?)null;
        bool keepAspect = !DontRespectAspectRatio.IsPresent;

        await ImagePlayground.ImageHelper.ResizeAsync(filePath, output, width, height, keepAspect, cancellationToken: CancelToken).ConfigureAwait(false);
    }
}
