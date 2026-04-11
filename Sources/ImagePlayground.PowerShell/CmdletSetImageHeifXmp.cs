using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Sets the XMP metadata packet in a HEIF or HEIC file.</summary>
/// <para>Requires an existing HEIF XMP item with a single writable file extent.</para>
/// <example>
///   <summary>Set a HEIC XMP packet from a string</summary>
///   <code>Set-ImageHeifXmp -FilePath photo.heic -Xmp $packet</code>
/// </example>
/// <example>
///   <summary>Set a HEIC XMP packet from a file</summary>
///   <code>Set-ImageHeifXmp -FilePath photo.heic -XmpPath packet.xmp -FilePathOutput photo-updated.heic</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageHeifXmp", DefaultParameterSetName = ParameterSetXmp)]
public sealed class SetImageHeifXmpCmdlet : PSCmdlet {
    private const string ParameterSetXmp = "Xmp";
    private const string ParameterSetXmpPath = "XmpPath";

    /// <summary>Path to the HEIF or HEIC file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetXmp)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetXmpPath)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional output path.</summary>
    /// <para>When not specified the source file is overwritten.</para>
    [Parameter(Position = 1, ParameterSetName = ParameterSetXmp)]
    [Parameter(Position = 1, ParameterSetName = ParameterSetXmpPath)]
    public string? FilePathOutput { get; set; }

    /// <summary>XMP metadata packet to write.</summary>
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetXmp)]
    public string? Xmp { get; set; }

    /// <summary>Path to a UTF-8 XMP metadata packet file.</summary>
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetXmpPath)]
    public string? XmpPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageHeifXmp - File {FilePath} not found. Please check the path.");
            return;
        }

        string xmp;
        if (ParameterSetName == ParameterSetXmpPath) {
            var xmpPath = Helpers.ResolvePath(XmpPath!);
            if (!File.Exists(xmpPath)) {
                WriteWarning($"Set-ImageHeifXmp - XMP file {XmpPath} not found. Please check the path.");
                return;
            }

            xmp = File.ReadAllText(xmpPath);
        } else {
            xmp = Xmp ?? string.Empty;
        }

        var output = string.IsNullOrWhiteSpace(FilePathOutput)
            ? filePath
            : Helpers.ResolvePath(FilePathOutput!);
        ImagePlayground.Image.SetHeifXmp(filePath, output, xmp);
    }
}
