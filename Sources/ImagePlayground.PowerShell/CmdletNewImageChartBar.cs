using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bar chart data item.</summary>
/// <para>Use this cmdlet inside <c>New-ImageChart</c> to define one bar series.</para>
/// <example>
///   <summary>Create bar-series data</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChartBar -Name 'Revenue' -Value 12,18,25 -Color Blue</code>
///   <para>Creates a single bar-series definition that can be passed into New-ImageChart.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBar")]
public sealed class NewImageChartBarCmdlet : PSCmdlet {
    /// <summary>Label for the bar data.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Values for the bar.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Bar color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBar(Name, Value, Color));
    }
}
