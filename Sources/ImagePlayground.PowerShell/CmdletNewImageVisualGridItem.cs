using ChartForgeX.Core;
using ChartForgeX.VisualBlocks;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one chart or visual-block placement for a visual grid.</summary>
[Cmdlet(VerbsCommon.New, "ImageVisualGridItem", DefaultParameterSetName = BlockSet)]
[OutputType(typeof(VisualGridItem))]
public sealed class NewImageVisualGridItemCmdlet : PSCmdlet {
    private const string BlockSet = "Block";
    private const string ChartSet = "Chart";

    /// <summary>Visual block hosted by the grid item.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = BlockSet)]
    public IVisualBlock? Block { get; set; }

    /// <summary>Chart hosted by the grid item.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ChartSet)]
    public Chart? Chart { get; set; }

    /// <summary>Optional stable motion target identifier.</summary>
    [Parameter]
    public string TargetId { get; set; } = string.Empty;

    /// <summary>Number of grid columns occupied by the item.</summary>
    [Parameter]
    [ValidateRange(1, 100)]
    public int ColumnSpan { get; set; } = 1;

    /// <summary>Number of grid rows occupied by the item.</summary>
    [Parameter]
    [ValidateRange(1, 100)]
    public int RowSpan { get; set; } = 1;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var hasTarget = !string.IsNullOrWhiteSpace(TargetId);
        if (Chart != null) WriteObject(hasTarget ? VisualGridItem.FromChart(TargetId, Chart, ColumnSpan, RowSpan) : VisualGridItem.FromChart(Chart, ColumnSpan, RowSpan));
        else WriteObject(hasTarget ? VisualGridItem.FromBlock(TargetId, Block!, ColumnSpan, RowSpan) : VisualGridItem.FromBlock(Block!, ColumnSpan, RowSpan));
    }
}
