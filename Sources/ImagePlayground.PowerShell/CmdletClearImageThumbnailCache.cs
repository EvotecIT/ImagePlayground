using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Clears cached thumbnails.</summary>
/// <example>
///   <summary>Remove every cached thumbnail</summary>
///   <code>Clear-ImageThumbnailCache</code>
/// </example>
[Cmdlet(VerbsCommon.Clear, "ImageThumbnailCache")]
public sealed class ClearImageThumbnailCacheCmdlet : PSCmdlet {
    /// <inheritdoc />
    protected override void ProcessRecord() {
        ImagePlayground.ImageHelper.ClearThumbnailCache();
    }
}
