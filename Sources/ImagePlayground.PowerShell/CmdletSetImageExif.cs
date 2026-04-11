using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground.PowerShell;

/// <summary>Sets an EXIF tag value in an image.</summary>
/// <para>The value must match the type declared by the selected EXIF tag, including ImageSharp wrapper types such as Number or Rational.</para>
/// <example>
///   <summary>Update DateTimeOriginal tag</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Set-ImageExif -FilePath img.jpg -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal) -Value (Get-Date)</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageExif")]
public sealed class SetImageExifCmdlet : ImageCmdlet {
    /// <summary>Image file to modify.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional output path.</summary>
    /// <para>When not specified the source file is overwritten.</para>
    [Parameter(Position = 1)]
    public string? FilePathOutput { get; set; }

    /// <summary>Tag to set.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public ExifTag ExifTag { get; set; } = null!;

    /// <summary>Value for the tag.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public object Value { get; set; } = null!;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "SetImageExifFileNotFound", FilePath);

        var value = Value;
        if (value is null) {
            throw new ArgumentNullException(nameof(Value));
        }

        Type expectedType = ExifTag.GetType().GenericTypeArguments[0];
        if (!expectedType.IsInstanceOfType(value)) {
            throw new ArgumentException(
                $"Value type '{value.GetType()}' does not match tag type '{expectedType}'.",
                nameof(Value));
        }

        var output = string.IsNullOrWhiteSpace(FilePathOutput) ? filePath : Helpers.ResolvePath(FilePathOutput!);
        ImagePlayground.Image.SetExifValue(filePath, output, ExifTag, value);
    }
}

