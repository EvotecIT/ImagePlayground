using System.Collections.Generic;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Core;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Renders a native ChartForgeX chart to an image or document file.</summary>
/// <para>ChartForgeX owns chart construction and options. This cmdlet only resolves the destination, saves the chart, and optionally opens it.</para>
/// <example>
///   <summary>Create and save a native ChartForgeX chart</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -ChartScript {
///     param($chart)
///     $points = [ChartForgeX.Core.ChartPoints]::FromValues(35, 42, 58, 61)
///     $chart.WithTitle('CPU').AddSmoothLine('Usage', $points).WithGrid()
/// } -FilePath cpu-usage.png</code>
///   <para>The script receives the native chart and may mutate it or return a replacement chart.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChart", DefaultParameterSetName = ChartScriptSet)]
public sealed class NewImageChartCmdlet : ImageCmdlet {
    private const string ChartSet = "Chart";
    private const string ChartScriptSet = "ChartScript";
    private readonly List<Chart> _charts = new();

    /// <summary>Script block that receives and configures a native ChartForgeX chart.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ChartScriptSet)]
    public ScriptBlock? ChartScript { get; set; }

    /// <summary>Native ChartForgeX chart object to render.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, ParameterSetName = ChartSet)]
    public Chart? Chart { get; set; }

    /// <summary>Output file path. The output format is inferred from its extension.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ChartScriptSet)]
    [Parameter(Mandatory = true, ParameterSetName = ChartSet)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the rendered file after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Chart != null) _charts.Add(Chart);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        if (ChartScript != null) {
            var chart = InvokeChartScript(ChartScript);
            SaveChart(chart);
            return;
        }

        if (_charts.Count != 1) {
            var message = _charts.Count == 0
                ? "New-ImageChart requires one native ChartForgeX.Core.Chart."
                : "New-ImageChart accepts one chart because -FilePath identifies one output.";
            var exception = new PSArgumentException(message);
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartInvalidChartCount", ErrorCategory.InvalidArgument, null));
            return;
        }

        SaveChart(_charts[0]);
    }

    private static Chart InvokeChartScript(ScriptBlock script) {
        var chart = ChartForgeX.Core.Chart.Create();
        var results = script.Invoke(chart);
        foreach (var result in results) {
            var value = result is PSObject psObject ? psObject.BaseObject : result;
            if (value is Chart replacement) chart = replacement;
        }

        return chart;
    }

    private void SaveChart(Chart chart) {
        var output = Helpers.ResolvePath(FilePath);
        chart.Save(output);
        if (Show.IsPresent) Helpers.Open(output, true);
    }
}
