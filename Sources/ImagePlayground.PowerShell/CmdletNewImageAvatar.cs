using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

[Cmdlet(VerbsCommon.New, "ImageAvatar", DefaultParameterSetName = ParameterSetPath)]
public sealed class NewImageAvatarCmdlet : PSCmdlet {
    private const string ParameterSetPath = "Path";
    private const string ParameterSetStream = "Stream";

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetPath)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetStream)]
    public string FilePath { get; set; } = string.Empty;

    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetPath)]
    public string OutputPath { get; set; } = string.Empty;

    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetStream)]
    public Stream OutputStream { get; set; } = Stream.Null;

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

        if (ParameterSetName == ParameterSetStream) {
            ImagePlayground.ImageHelper.Avatar(FilePath, OutputStream, Width, Height, CornerRadius);
            OutputStream.Position = 0;
        } else {
            ImagePlayground.ImageHelper.Avatar(FilePath, OutputPath, Width, Height, CornerRadius);
            if (Open.IsPresent) {
                ImagePlayground.Helpers.Open(OutputPath, true);
            }
        }
    }
}
