using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bar chart options.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartBarOptions")]
public sealed class NewImageChartBarOptionsCmdlet : PSCmdlet {
    /// <summary>Display values above bars.</summary>
    [Parameter]
    public SwitchParameter ShowValuesAboveBars { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImagePlayground.Charts.ChartBarOptions(ShowValuesAboveBars.IsPresent));
    }
}
