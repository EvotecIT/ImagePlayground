using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

[Cmdlet(VerbsCommon.New, "ImageAvatar")]
public sealed class NewImageAvatarCmdlet : PSCmdlet {
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 200;

    [Parameter]
    public int Height { get; set; } = 200;

    [Parameter]
    public float CornerRadius { get; set; } = 20f;

    [Parameter]
    public SwitchParameter Open { get; set; }

    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"New-ImageAvatar - File {FilePath} not found. Please check the path.");
            return;
        }

        ImagePlayground.ImageHelper.Avatar(FilePath, OutputPath, Width, Height, CornerRadius);

        if (Open.IsPresent) {
            ImagePlayground.Helpers.Open(OutputPath, true);
        }
    }
}
