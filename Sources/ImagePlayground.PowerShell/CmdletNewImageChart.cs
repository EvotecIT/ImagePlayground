using ImagePlayground;
using System.Collections.Generic;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an image chart from definitions.</summary>
[Cmdlet(VerbsCommon.New, "ImageChart")]
public sealed class NewImageChartCmdlet : PSCmdlet {
    /// <summary>ScriptBlock producing chart definitions.</summary>
    [Parameter(Position = 0)]
    public ScriptBlock? ChartsDefinition { get; set; }

    /// <summary>Chart definitions provided directly.</summary>
    [Parameter(ValueFromPipeline = true)]
    public Charts.ChartDefinition[]? Definition { get; set; }

    /// <summary>Width of the chart.</summary>
    [Parameter]
    public int Width { get; set; } = 600;

    /// <summary>Height of the chart.</summary>
    [Parameter]
    public int Height { get; set; } = 400;

    /// <summary>X axis title.</summary>
    [Parameter]
    public string? XTitle { get; set; }

    /// <summary>Y axis title.</summary>
    [Parameter]
    public string? YTitle { get; set; }

    /// <summary>Output file path.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Display grid lines.</summary>
    [Parameter]
    public SwitchParameter ShowGrid { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var list = new List<Charts.ChartDefinition>();
        if (Definition is not null) {
            list.AddRange(Definition);
        }
        if (ChartsDefinition != null) {
            var results = ChartsDefinition.Invoke();
            foreach (var o in results) {
                var obj = o is PSObject ps ? ps.BaseObject : o;
                if (obj is Charts.ChartDefinition def) {
                    list.Add(def);
                }
            }
        }
        if (list.Count == 0) {
            WriteWarning("New-ImageChart - No chart definitions specified.");
            return;
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.Charts.Generate(list, output, Width, Height, null, XTitle, YTitle, ShowGrid.IsPresent);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}
