using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Resizes an image object or a source file.</summary>
/// <para>Width and height bound the resized image while preserving its aspect ratio. Object input is updated and emitted for further edits or saving. Path input requires OutputPath and saves the result.</para>
/// <example>
///   <summary>Resize and save an object pipeline</summary>
///   <code>Get-Image in.png | Resize-Image -Width 1200 | Save-Image -FilePath out.png</code>
/// </example>
/// <example>
///   <summary>Fit a file inside a 100x100 box</summary>
///   <code>Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100</code>
/// </example>
/// <example>
///   <summary>Double the size of an image object</summary>
///   <code>$image | Resize-Image -Percentage 200</code>
/// </example>
[Cmdlet(VerbsCommon.Resize, "Image", DefaultParameterSetName = FileDimensions)]
[OutputType(typeof(ImagePlayground.Image))]
public sealed class ResizeImageCmdlet : AsyncImageCmdlet {
    private const string FileDimensions = "FileDimensions";
    private const string FilePercentage = "FilePercentage";
    private const string ObjectDimensions = "ObjectDimensions";
    private const string ObjectPercentage = "ObjectPercentage";

    /// <summary>Source path, resolved in the current PowerShell filesystem location.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = FileDimensions)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = FilePercentage)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Editable object; the same instance is emitted after resizing.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ObjectDimensions)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ObjectPercentage)]
    public ImagePlayground.Image Image { get; set; } = null!;

    /// <summary>Destination path; required for path input and optional for object input.</summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = FileDimensions)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = FilePercentage)]
    [Parameter(Position = 1, ParameterSetName = ObjectDimensions)]
    [Parameter(Position = 1, ParameterSetName = ObjectPercentage)]
    public string? OutputPath { get; set; }

    /// <summary>Requested width or maximum width when both bounds are supplied.</summary>
    /// <para>The shared owner validates pixel and working-memory limits before allocation.</para>
    [Parameter(ParameterSetName = FileDimensions)]
    [Parameter(ParameterSetName = ObjectDimensions)]
    [ValidateRange(1, int.MaxValue)]
    public int Width { get; set; }

    /// <summary>Requested height or maximum height when both bounds are supplied.</summary>
    [Parameter(ParameterSetName = FileDimensions)]
    [Parameter(ParameterSetName = ObjectDimensions)]
    [ValidateRange(1, int.MaxValue)]
    public int Height { get; set; }

    /// <summary>Positive percentage for uniform scaling relative to the original dimensions.</summary>
    [Parameter(Mandatory = true, ParameterSetName = FilePercentage)]
    [Parameter(Mandatory = true, ParameterSetName = ObjectPercentage)]
    [ValidateRange(1, int.MaxValue)]
    public int Percentage { get; set; }

    /// <summary>Stretches to the supplied dimensions; an omitted dimension retains its original value.</summary>
    [Parameter(ParameterSetName = FileDimensions)]
    [Parameter(ParameterSetName = ObjectDimensions)]
    public SwitchParameter DontRespectAspectRatio { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        bool objectInput = ParameterSetName == ObjectDimensions || ParameterSetName == ObjectPercentage;
        bool percentage = ParameterSetName == FilePercentage || ParameterSetName == ObjectPercentage;
        bool widthBound = MyInvocation.BoundParameters.ContainsKey(nameof(Width));
        bool heightBound = MyInvocation.BoundParameters.ContainsKey(nameof(Height));
        bool keepAspect = !DontRespectAspectRatio.IsPresent;
        if (!percentage && !widthBound && !heightBound) {
            ThrowTerminatingError(new ErrorRecord(new PSArgumentException("Width or Height or Percentage must be specified."), "ResizeImageMissingDimensions", ErrorCategory.InvalidArgument, null));
            return;
        }
        int? width = widthBound ? Width : (int?)null;
        int? height = heightBound ? Height : (int?)null;
        string? output = string.IsNullOrWhiteSpace(OutputPath) ? null : PowerShellPathResolver.ResolveFileSystemPath(this, OutputPath!);
        string? source = objectInput ? null : ResolveExistingFilePath(FilePath, "ResizeImageFileNotFound", FilePath);
        if (objectInput) {
            if (percentage) { Image.Resize(Percentage, CancelToken); }
            else { Image.Resize(width, height, keepAspect, CancelToken); }
            if (output != null) { await Image.SaveAsync(output, cancellationToken: CancelToken).ConfigureAwait(false); }
            WriteObject(Image);
        } else if (percentage) {
            await ImageHelper.ResizeAsync(source!, output!, Percentage, CancelToken).ConfigureAwait(false);
        } else {
            await ImageHelper.ResizeAsync(source!, output!, width, height, keepAspect, cancellationToken: CancelToken).ConfigureAwait(false);
        }
    }
}
