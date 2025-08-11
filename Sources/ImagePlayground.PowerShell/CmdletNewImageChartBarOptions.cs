using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bar chart options.</summary>
/// <example>
///   <summary>Show values above bars</summary>
///   <code>New-ImageChartBarOptions -ShowValuesAboveBars</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBarOptions")]
public sealed class NewImageChartBarOptionsCmdlet : PSCmdlet {
    /// <summary>Display values above bars.</summary>
    [Parameter]
    public SwitchParameter ShowValuesAboveBars { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBarOptions(ShowValuesAboveBars.IsPresent));
    }
}
