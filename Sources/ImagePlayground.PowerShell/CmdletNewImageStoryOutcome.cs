using System.Management.Automation;
using ImagePlayground.PowerShell.Stories;

namespace ImagePlayground.PowerShell;

/// <summary>Declares a result that must be visible in the completed visual-story scene.</summary>
[Cmdlet(VerbsCommon.New, "ImageStoryOutcome")]
[OutputType(typeof(ImageStoryOutcomeSpec))]
public sealed class NewImageStoryOutcomeCmdlet : PSCmdlet {
    /// <summary>Stable outcome identifier.</summary>
    [Parameter(Mandatory = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Human-readable outcome label.</summary>
    [Parameter(Mandatory = true)]
    public string Label { get; set; } = string.Empty;

    /// <summary>Panel identifier that must be present in the completed scene.</summary>
    [Parameter(Mandatory = true)]
    public string PanelId { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImageStoryOutcomeSpec(Id, Label, PanelId));
    }
}
