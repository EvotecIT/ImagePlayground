using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

[Cmdlet(VerbsData.ConvertFrom, "ImageBase64")]
public sealed class ConvertFromImageBase64Cmdlet : PSCmdlet {
    [Parameter(Mandatory = true, Position = 0)]
    public string Base64 { get; set; } = string.Empty;

    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    [Parameter]
    public SwitchParameter Open { get; set; }

    protected override void ProcessRecord() {
        var output = Helpers.ResolvePath(OutputPath);
        ImageHelper.ConvertFromBase64(Base64, output);
        Helpers.Open(output, Open.IsPresent);
    }
}
