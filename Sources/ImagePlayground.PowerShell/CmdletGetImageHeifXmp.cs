using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Gets the XMP metadata packet from a HEIF or HEIC file.</summary>
/// <para>Returns the UTF-8 XMP packet when the container declares an XMP MIME metadata item.</para>
/// <example>
///   <summary>Read a HEIC XMP packet</summary>
///   <code>Get-ImageHeifXmp -FilePath photo.heic</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageHeifXmp")]
[OutputType(typeof(string))]
public sealed class GetImageHeifXmpCmdlet : PSCmdlet {
    /// <summary>Path to the HEIF or HEIC file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Get-ImageHeifXmp - File {FilePath} not found. Please check the path.");
            return;
        }

        string? xmp = ImagePlayground.Image.GetHeifXmp(filePath);
        if (xmp is not null) {
            WriteObject(xmp);
        }
    }
}
