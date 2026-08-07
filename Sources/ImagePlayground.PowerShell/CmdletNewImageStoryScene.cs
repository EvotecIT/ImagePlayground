using System.Management.Automation;
using ChartForgeX.Stories;
using ImagePlayground.PowerShell.Stories;

namespace ImagePlayground.PowerShell;

/// <summary>Groups resolved panels into one timed visual-story scene.</summary>
/// <example>
///   <summary>Create a scene from a resolved text panel</summary>
///   <code>$panel = New-ImageStoryPanel -Id 'summary' -Text 'Deployment completed.' -Emphasized
/// New-ImageStoryScene -Id 'result' -Title 'Deployment result' -Panels $panel -DurationSeconds 3</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageStoryScene")]
[OutputType(typeof(ImageStorySceneSpec))]
public sealed class NewImageStorySceneCmdlet : PSCmdlet {
    private readonly List<ImageStoryPanelSpec> _panels = new();

    /// <summary>Stable scene identifier.</summary>
    [Parameter(Mandatory = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Visible scene heading.</summary>
    [Parameter(Mandatory = true)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Resolved panels in display order.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public ImageStoryPanelSpec[] Panels { get; set; } = System.Array.Empty<ImageStoryPanelSpec>();

    /// <summary>Panel arrangement.</summary>
    [Parameter]
    public VisualStorySceneLayout Layout { get; set; } = VisualStorySceneLayout.Focus;

    /// <summary>Scene display duration.</summary>
    [Parameter]
    [PSDefaultValue(Value = 2.5, Help = "2.5")]
    [ValidateRange(0.25, 60)]
    public double DurationSeconds { get; set; } = 2.5;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        foreach (var panel in Panels) {
            _panels.Add(panel);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        WriteObject(new ImageStorySceneSpec(Id, Title, DurationSeconds, Layout, _panels.ToArray()));
    }
}
