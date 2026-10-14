using System.Management.Automation;
using ChartForgeX.Primitives;
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
/// <example>
///   <summary>Bars and a target line on one chart</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -Title 'Builds' -XLabel Mon, Tue, Wed -FilePath builds.png {
///     New-ImageChartBar -Name Passed -Value 41, 38, 45
///     New-ImageChartLine -Name Target -Value 40, 40, 40 -Color '#c78404'
/// }</code>
///   <para>Bar and line definitions can be mixed in one chart.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBar")]
[OutputType(typeof(ChartBar))]
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
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Plot the bars against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.</summary>
    [Parameter]
    public SwitchParameter SecondaryAxis { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBar(Name, Value, ChartColorConverter.Convert(Color)) { SecondaryAxis = SecondaryAxis.IsPresent });
    }
}
