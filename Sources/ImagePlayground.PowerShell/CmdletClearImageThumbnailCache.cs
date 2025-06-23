using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Clears cached thumbnails.</summary>
[Cmdlet(VerbsCommon.Clear, "ImageThumbnailCache")]
public sealed class ClearImageThumbnailCacheCmdlet : PSCmdlet {
    /// <inheritdoc />
    protected override void ProcessRecord() {
        ImagePlayground.ImageHelper.ClearThumbnailCache();
    }
}
