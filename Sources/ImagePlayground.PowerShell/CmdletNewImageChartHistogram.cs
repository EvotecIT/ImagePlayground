using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates histogram chart data item.</summary>
/// <example>
///   <summary>Create histogram data</summary>
///   <code>New-ImageChartHistogram -Name 'Data' -Values @(1,2,3) -BinSize 2</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartHistogram")]
public sealed class NewImageChartHistogramCmdlet : PSCmdlet {
    /// <summary>Label for the histogram.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Data values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Values { get; set; } = System.Array.Empty<double>();

    /// <summary>Optional bin size.</summary>
    [Parameter]
    public int? BinSize { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartHistogram(Name, Values, BinSize));
    }
}
