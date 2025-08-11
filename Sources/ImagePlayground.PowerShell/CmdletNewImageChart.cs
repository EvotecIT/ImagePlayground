using ImagePlayground;
using System.Collections.Generic;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an image chart from definitions.</summary>
/// <example>
///   <summary>Create a bar chart and save it</summary>
///   <code>New-ImageChart -ChartsDefinition { New-ImageChartBar -Name 'Jan' -Value @(1,2) } -FilePath chart.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChart")]
public sealed class NewImageChartCmdlet : PSCmdlet {
    private const string ScriptBlockSet = "ScriptBlock";
    private const string DefinitionSet = "Definition";

    /// <summary>ScriptBlock producing chart definitions.</summary>
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = ScriptBlockSet)]
    public ScriptBlock? ChartsDefinition { get; set; }

    /// <summary>Chart definitions provided directly.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, ParameterSetName = DefinitionSet)]
    public ChartDefinition[]? Definition { get; set; }

    /// <summary>ScriptBlock producing annotations.</summary>
    [Parameter]
    public ScriptBlock? AnnotationsDefinition { get; set; }

    /// <summary>Annotations for the chart.</summary>
    [Parameter]
    public ChartAnnotation[]? Annotation { get; set; }

    /// <summary>Width of the chart.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Width { get; set; } = 600;

    /// <summary>Height of the chart.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Height { get; set; } = 400;

    /// <summary>X axis title.</summary>
    [Parameter]
    public string? XTitle { get; set; }

    /// <summary>Y axis title.</summary>
    [Parameter]
    public string? YTitle { get; set; }

    /// <summary>Output file path.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ScriptBlockSet)]
    [Parameter(Mandatory = true, ParameterSetName = DefinitionSet)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Display grid lines.</summary>
    [Parameter]
    public SwitchParameter ShowGrid { get; set; }

    /// <summary>Chart theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <summary>Chart background color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Background { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var list = new List<ChartDefinition>();
        if (Definition is not null) {
            list.AddRange(Definition);
        }
        if (ChartsDefinition != null) {
            var results = ChartsDefinition.Invoke();
            foreach (var o in results) {
                var obj = o is PSObject ps ? ps.BaseObject : o;
                if (obj is ChartDefinition def) {
                    list.Add(def);
                }
            }
        }
        if (list.Count == 0) {
            WriteWarning("New-ImageChart - No chart definitions specified.");
            return;
        }

        var annotations = new List<ChartAnnotation>();
        if (Annotation is not null) {
            annotations.AddRange(Annotation);
        }
        if (AnnotationsDefinition != null) {
            var ares = AnnotationsDefinition.Invoke();
            foreach (var o in ares) {
                var obj = o is PSObject ps ? ps.BaseObject : o;
                if (obj is ChartAnnotation ann) {
                    annotations.Add(ann);
                }
            }
        }

        var output = Helpers.ResolvePath(FilePath);
        Charts.Generate(list, output, Width, Height, null, XTitle, YTitle, ShowGrid.IsPresent, Theme, annotations, Background);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}
