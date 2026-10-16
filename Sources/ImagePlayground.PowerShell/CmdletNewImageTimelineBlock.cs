using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an activity timeline block.</summary>
/// <example>
///   <summary>Create a release activity feed</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTimelineBlock -Title 'Release activity' -ItemDefinition { New-ImageTimelineItem -Kind Event -Title 'Build completed' -Status Positive; New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests' -Completed }</code>
/// </example>
/// <example>
///   <summary>Pipe timeline items and size the block</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$deployments | ForEach-Object { New-ImageTimelineItem -Kind Event -Title $_.Name -Timestamp $_.Time -Status Positive } | New-ImageTimelineBlock -Title Deployments -Width 420 -Height 300</code>
///   <para>Items can arrive through the pipeline; Width and Height size the block for a grid panel.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTimelineBlock")]
[OutputType(typeof(ActivityTimelineBlock))]
public sealed class NewImageTimelineBlockCmdlet : PSCmdlet {
    private readonly List<ActivityTimelineItem> _items = new();

    /// <summary>Timeline items supplied directly or through the pipeline.</summary>
    [Parameter(ValueFromPipeline = true)]
    public ActivityTimelineItem[] Item { get; set; } = Array.Empty<ActivityTimelineItem>();

    /// <summary>Script block that emits items from <c>New-ImageTimelineItem</c>.</summary>
    [Parameter]
    public ScriptBlock? ItemDefinition { get; set; }

    /// <summary>Optional block title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional block subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>Render event rows without card-like surfaces.</summary>
    [Parameter]
    public SwitchParameter Compact { get; set; }

    /// <summary>Block width in pixels. The height keeps its default unless -Height is also supplied.</summary>
    [Parameter]
    [ValidateRange(1, VisualBlockSize.Maximum)]
    public int Width { get; set; }

    /// <summary>Block height in pixels. The width keeps its default unless -Width is also supplied.</summary>
    [Parameter]
    [ValidateRange(1, VisualBlockSize.Maximum)]
    public int Height { get; set; }

    /// <summary>ChartForgeX theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        foreach (var item in Item) {
            if (item != null) _items.Add(item);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var items = new List<ActivityTimelineItem>(_items);
        if (ItemDefinition != null) {
            foreach (var result in ItemDefinition.Invoke()) {
                var value = result is PSObject psObject ? psObject.BaseObject : result;
                if (value is ActivityTimelineItem item) items.Add(item);
                else if (value != null) {
                    var exception = new PSArgumentException("ItemDefinition must emit only New-ImageTimelineItem results.");
                    ThrowTerminatingError(new ErrorRecord(exception, "NewImageTimelineBlockUnsupportedItem", ErrorCategory.InvalidArgument, value));
                }
            }
        }
        if (items.Count == 0) {
            var exception = new PSArgumentException("An activity timeline requires at least one item.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageTimelineBlockMissingItem", ErrorCategory.InvalidArgument, null));
        }
        var block = ActivityTimelineBlock.Create()
            .WithEventSurfaces(!Compact.IsPresent)
            .WithTheme(ChartThemeResolver.Resolve(Theme));
        if (!string.IsNullOrWhiteSpace(Title)) block.WithTitle(Title);
        if (!string.IsNullOrWhiteSpace(Subtitle)) block.WithSubtitle(Subtitle);
        foreach (var item in items) AddItem(block, item);
        VisualBlockSize.Apply(this, block, Width, Height);
        WriteObject(block);
    }

    private static void AddItem(ActivityTimelineBlock block, ActivityTimelineItem item) {
        switch (item.Kind) {
            case ActivityTimelineItemKind.Section:
                block.AddSection(item.Title);
                break;
            case ActivityTimelineItemKind.ChecklistItem:
                block.AddChecklistItem(item.Title, item.Completed, item.Muted);
                break;
            case ActivityTimelineItemKind.HiddenSummary:
                block.AddHiddenSummary(item.HiddenCount, item.Title);
                break;
            default:
                block.AddEvent(item.Title, item.Timestamp, item.Status, item.Badge, item.Detail, item.Symbol);
                break;
        }
    }
}
