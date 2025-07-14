using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground.PowerShell;

/// <summary>Sets an EXIF tag value in an image.</summary>
/// <example>
///   <summary>Update DateTimeOriginal tag</summary>
///   <code>Set-ImageExif -FilePath img.jpg -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal) -Value (Get-Date)</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageExif")]
public sealed class SetImageExifCmdlet : PSCmdlet {
    /// <summary>Image file to modify.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional output path. When not specified the file is overwritten.</summary>
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
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageExif - File not found: {FilePath}");
            return;
        }

        Type expectedType = ExifTag.GetType().GenericTypeArguments[0];
        if (Value is not null && !expectedType.IsInstanceOfType(Value)) {
            throw new ArgumentException(
                $"Value type '{Value.GetType()}' does not match tag type '{expectedType}'.",
                nameof(Value));
        }

        using var img = ImagePlayground.Image.Load(filePath);
        img.SetExifValue(ExifTag, Value);

        var output = string.IsNullOrWhiteSpace(FilePathOutput) ? filePath : Helpers.ResolvePath(FilePathOutput!);
        img.Save(output);
    }
}

