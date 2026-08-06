using ChartForgeX.VisualBlocks;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one activity timeline item.</summary>
/// <example>
///   <summary>Create a deployment event</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTimelineItem -Kind Event -Title 'Production deployed' -Timestamp '14:32' -Status Positive</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTimelineItem")]
[OutputType(typeof(ActivityTimelineItem))]
public sealed class NewImageTimelineItemCmdlet : PSCmdlet {
    /// <summary>Timeline item kind.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public ActivityTimelineItemKind Kind { get; set; }

    /// <summary>Primary item text.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional timestamp text for events.</summary>
    [Parameter]
    public string Timestamp { get; set; } = string.Empty;

    /// <summary>Event status.</summary>
    [Parameter]
    public VisualStatus Status { get; set; } = VisualStatus.Neutral;

    /// <summary>Optional compact event badge.</summary>
    [Parameter]
    public string Badge { get; set; } = string.Empty;

    /// <summary>Optional event detail.</summary>
    [Parameter]
    public string Detail { get; set; } = string.Empty;

    /// <summary>Optional compact event symbol.</summary>
    [Parameter]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Mark a checklist item as completed.</summary>
    [Parameter]
    public SwitchParameter Completed { get; set; }

    /// <summary>Render checklist text as muted.</summary>
    [Parameter]
    public SwitchParameter Muted { get; set; }

    /// <summary>Number of collapsed items represented by a hidden summary.</summary>
    [Parameter]
    [ValidateRange(0, int.MaxValue)]
    public int HiddenCount { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        ActivityTimelineItem item;
        switch (Kind) {
            case ActivityTimelineItemKind.Section:
                item = ActivityTimelineItem.Section(Title);
                break;
            case ActivityTimelineItemKind.ChecklistItem:
                item = ActivityTimelineItem.Checklist(Title, Completed.IsPresent, Muted.IsPresent);
                break;
            case ActivityTimelineItemKind.HiddenSummary:
                item = ActivityTimelineItem.Hidden(HiddenCount, Title);
                break;
            default:
                item = ActivityTimelineItem.Event(Title, string.IsNullOrWhiteSpace(Timestamp) ? null : Timestamp, Status, string.IsNullOrWhiteSpace(Badge) ? null : Badge, string.IsNullOrWhiteSpace(Detail) ? null : Detail, string.IsNullOrWhiteSpace(Symbol) ? null : Symbol);
                break;
        }
        WriteObject(item);
    }
}
