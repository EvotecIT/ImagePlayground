using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a visual list block.</summary>
/// <example>
///   <summary>Create a checklist</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageListBlock -Title 'Release' -Item Build,Test,Publish -Checked $true,$true,$false</code>
/// </example>
/// <example>
///   <summary>Size a status list for a dashboard column</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageListBlock -Title Checks -Item 'TLS expiry', Backups, 'Queue depth' -Value '41 days', OK, '1,204' -Status Positive, Positive, Warning -Width 300 -Height 270</code>
///   <para>Width and Height size the block without calling .NET methods on the result.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageListBlock")]
[OutputType(typeof(ChartList))]
public sealed class NewImageListBlockCmdlet : PSCmdlet {
    /// <summary>List item text.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string[] Item { get; set; } = Array.Empty<string>();

    /// <summary>Optional right-aligned values matching the item count.</summary>
    [Parameter]
    public string[] Value { get; set; } = Array.Empty<string>();

    /// <summary>Optional semantic statuses matching the item count.</summary>
    [Parameter]
    public VisualStatus[] Status { get; set; } = Array.Empty<VisualStatus>();

    /// <summary>Optional checked states matching the item count.</summary>
    [Parameter]
    public bool[] Checked { get; set; } = Array.Empty<bool>();

    /// <summary>Optional block title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional block subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>List marker style.</summary>
    [Parameter]
    public VisualListMarker Marker { get; set; } = VisualListMarker.Bullet;

    /// <summary>Use compact row spacing.</summary>
    [Parameter]
    public SwitchParameter Dense { get; set; }

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
        ValidateOptionalCount(Value.Length, nameof(Value));
        ValidateOptionalCount(Status.Length, nameof(Status));
        ValidateOptionalCount(Checked.Length, nameof(Checked));
        var list = ChartList.Create()
            .WithMarker(Marker)
            .WithDenseMode(Dense.IsPresent)
            .WithTheme(ChartThemeResolver.Resolve(Theme), ChartThemeResolver.ResolveMode(Theme));
        if (!string.IsNullOrWhiteSpace(Title)) list.WithTitle(Title);
        if (!string.IsNullOrWhiteSpace(Subtitle)) list.WithSubtitle(Subtitle);
        for (var index = 0; index < Item.Length; index++) {
            var value = Value.Length == 0 ? null : Value[index];
            if (Checked.Length > 0) list.AddCheckItem(Item[index], Checked[index], value);
            else if (Status.Length > 0) list.AddStatusItem(Item[index], Status[index], value);
            else list.AddItem(Item[index], value);
        }
        VisualBlockSize.Apply(this, list, Width, Height);
        WriteObject(list);
    }

    private void ValidateOptionalCount(int count, string parameterName) {
        if (count == 0 || count == Item.Length) return;
        var exception = new PSArgumentException(parameterName + " must be empty or match the Item count.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageListBlockMismatchedCount", ErrorCategory.InvalidArgument, parameterName));
    }
}
