using ImagePlayground;
using System.IO;
using System.Management.Automation;
using OfficeIMO.Drawing;

namespace ImagePlayground.PowerShell;

/// <summary>Sets an EXIF tag value in an image.</summary>
/// <para>The shared metadata API validates the selected tag and its value. Use OfficeRational values for unsigned EXIF fractions.</para>
/// <example>
///   <summary>Update DateTimeOriginal tag</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Set-ImageExif -FilePath img.jpg -ExifTag ([OfficeIMO.Drawing.OfficeExifTag]::DateTimeOriginal) -Value (Get-Date -Format 'yyyy:MM:dd HH:mm:ss')</code>
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
    public OfficeExifTag ExifTag { get; set; }

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

        var output = string.IsNullOrWhiteSpace(FilePathOutput) ? filePath : PowerShellPathResolver.ResolveFileSystemPath(this, FilePathOutput!);
        ImagePlayground.Image.SetExifValue(filePath, output, ExifTag, value);
    }
}

