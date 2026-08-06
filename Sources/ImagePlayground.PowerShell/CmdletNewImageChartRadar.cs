using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates radar chart series data.</summary>
/// <para>Use at least three category coordinates. Multiple emitted definitions become multiple radar series in <c>New-ImageChart</c>.</para>
/// <example>
///   <summary>Create a radar chart</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -ChartsDefinition { New-ImageChartRadar -Name 'Current' -Category 1,2,3,4 -Value 82,68,91,74 -Color '#2563EB' } -FilePath radar.svg</code>
///   <para>Creates a four-axis radar chart using the shared ChartForgeX renderer.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartRadar")]
[OutputType(typeof(ChartRadar))]
public sealed class NewImageChartRadarCmdlet : PSCmdlet {
    /// <summary>Series name.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Numeric category coordinates shared by the radar series.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Category { get; set; } = System.Array.Empty<double>();

    /// <summary>Values for the radar series.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Series color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartRadar(Name, Category, Value, ChartColorConverter.Convert(Color)));
    }
}
