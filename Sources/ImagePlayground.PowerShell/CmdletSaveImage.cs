using ImagePlayground;
using OfficeIMO.Drawing;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Saves an image object to disk or returns its encoded bytes as a stream.</summary>
/// <para>Accepts editable image objects from Get-Image and Resize-Image.</para>
/// <example>
///   <summary>Resize and save an image object</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Get-Image in.png | Resize-Image -Width 1200 | Save-Image -FilePath out.png</code>
/// </example>
/// <example>
///   <summary>Return an explicit PNG stream</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Get-Image in.jpg | Save-Image -AsStream -Format Png</code>
/// </example>
[Cmdlet(VerbsData.Save, "Image", DefaultParameterSetName = "File")]
public sealed class SaveImageCmdlet : AsyncImageCmdlet {
    /// <summary>Editable image object to save.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public ImagePlayground.Image Image { get; set; } = null!;

    /// <summary>Optional destination path; omitted uses the path associated with the image.</summary>
    [Parameter(Position = 1, ParameterSetName = "File")]
    public string? FilePath { get; set; }

    /// <summary>Quality for JPEG or WEBP images.</summary>
    [Parameter]
    public int? Quality { get; set; }

    /// <summary>Compression level for PNG images.</summary>
    [Parameter]
    public int? CompressionLevel { get; set; }

    /// <summary>Owned encoder settings; simple quality and compression controls override their matching settings.</summary>
    [Parameter]
    public OfficeRasterEncodingOptions? EncodingOptions { get; set; }

    /// <summary>Returns an encoded stream at position zero; the caller owns the stream.</summary>
    [Parameter(Mandatory = true, ParameterSetName = "Stream")]
    public SwitchParameter AsStream { get; set; }

    /// <summary>Explicit stream output format; omitted uses the image's detected supported default.</summary>
    [Parameter(ParameterSetName = "Stream")]
    public ImageType? Format { get; set; }

    /// <summary>Opens the completed file.</summary>
    [Parameter(ParameterSetName = "File")]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        string? output = string.IsNullOrWhiteSpace(FilePath) ? null : PowerShellPathResolver.ResolveFileSystemPath(this, FilePath!);
        bool asStream = AsStream.IsPresent;
        bool open = Open.IsPresent;
        if (!asStream && output == null && string.IsNullOrWhiteSpace(Image.FilePath)) {
            ThrowTerminatingError(new ErrorRecord(new PSArgumentException("FilePath is required when the image has no associated file path."), "SaveImageMissingPath", ErrorCategory.InvalidArgument, Image));
            return;
        }
        ImageType format = asStream ? Format ?? Image.DefaultOutputFormat : Helpers.GetImageType(System.IO.Path.GetExtension(output ?? Image.FilePath));
        var options = EncodingOptions?.Clone() ?? new OfficeRasterEncodingOptions();
        var simple = Helpers.GetEncodingOptions(format, Quality, CompressionLevel);
        if (Quality.HasValue) {
            options.Jpeg.Quality = simple.Jpeg.Quality;
            if (format == ImageType.WebP) { options.Webp.Mode = simple.Webp.Mode; options.Webp.Quality = simple.Webp.Quality; }
        }
        if (CompressionLevel.HasValue) { options.Png.Compression = simple.Png.Compression; }
        if (asStream) {
            WriteObject(Image.ToStream(format, options, CancelToken));
        } else {
            await Image.SaveAsync(output ?? "", options, open, CancelToken).ConfigureAwait(false);
        }
    }
}
