using ChartForgeX.Topology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an organization or team chart.</summary>
/// <para>Maps PowerShell-authored team members into the shared ChartForgeX hierarchy engine, including compact child buckets and orthogonal parent-child routing.</para>
/// <example>
///   <summary>Create an engineering organization chart</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageOrganizationChart -TeamId engineering -TeamLabel 'Engineering' -MemberDefinition { New-ImageOrganizationMember -Id lead -Name 'Avery Stone' -Role 'Director'; New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId lead } -FilePath engineering.svg</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageOrganizationChart", DefaultParameterSetName = DefinitionSet)]
[OutputType(typeof(TopologyChart))]
public sealed class NewImageOrganizationChartCmdlet : PSCmdlet {
    private const string DefinitionSet = "Definition";
    private const string MemberSet = "Member";
    private readonly List<TopologyTeamMember> _members = new();

    /// <summary>Script block that emits members from <c>New-ImageOrganizationMember</c>.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = DefinitionSet)]
    public ScriptBlock? MemberDefinition { get; set; }

    /// <summary>Members supplied directly or through the pipeline.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = MemberSet)]
    public TopologyTeamMember[] Member { get; set; } = Array.Empty<TopologyTeamMember>();

    /// <summary>Stable team root identifier.</summary>
    [Parameter]
    public string TeamId { get; set; } = "organization";

    /// <summary>Team or organization label.</summary>
    [Parameter(Mandatory = true)]
    public string TeamLabel { get; set; } = string.Empty;

    /// <summary>Optional chart subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>Output width in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 1200;

    /// <summary>Output height in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 700;

    /// <summary>Layered organization-chart direction.</summary>
    [Parameter]
    public TopologyLayoutDirection Direction { get; set; } = TopologyLayoutDirection.TopToBottom;

    /// <summary>Organization member presentation mode.</summary>
    [Parameter]
    public TopologyNodeDisplayMode NodeDisplayMode { get; set; } = TopologyNodeDisplayMode.Card;

    /// <summary>Lowest primary hierarchy level to include.</summary>
    [Parameter]
    public int MinLevel { get; set; }

    /// <summary>Highest hierarchy level to include.</summary>
    [Parameter]
    public int MaxLevel { get; set; }

    /// <summary>Omit the generated team root node.</summary>
    [Parameter]
    public SwitchParameter NoTeamNode { get; set; }

    /// <summary>Omit ancestors below the requested minimum level.</summary>
    [Parameter]
    public SwitchParameter NoAncestorContext { get; set; }

    /// <summary>Show the node-kind and status legend.</summary>
    [Parameter]
    public SwitchParameter ShowLegend { get; set; }

    /// <summary>Topology theme name.</summary>
    [Parameter]
    [ValidateSet("Light", "Dark")]
    public string Theme { get; set; } = "Light";

    /// <summary>Use a transparent chart background.</summary>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <summary>Output file path. Supported extensions are PNG, SVG, HTML, and HTM.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the generated organization chart.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the generated ChartForgeX topology chart to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Member.Length > 0) _members.AddRange(Member);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        ValidateExtension(Path.GetExtension(output), output);
        PowerShellPathResolver.ValidateFileDestination(output, nameof(FilePath), nameof(FilePath));
        if (MemberDefinition != null) AddDefinitionResults(MemberDefinition.Invoke());
        if (_members.Count == 0) {
            var exception = new PSArgumentException("An organization chart requires at least one member.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageOrganizationChartMissingMember", ErrorCategory.InvalidArgument, null));
        }

        var chart = TopologyChart.Create();
        chart.Title = TeamLabel;
        chart.Subtitle = string.IsNullOrWhiteSpace(Subtitle) ? null : Subtitle;
        chart.Viewport.Width = Width;
        chart.Viewport.Height = Height;
        chart.Theme = Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase) ? TopologyTheme.Dark() : TopologyTheme.Light();
        if (Transparent.IsPresent) chart.Theme.Background = "#FFFFFF00";

        var teamOptions = new TopologyTeamOptions {
            IncludeTeamNode = !NoTeamNode.IsPresent,
            IncludeAncestorContext = !NoAncestorContext.IsPresent,
            LayoutDirection = Direction,
            NodeDisplayMode = NodeDisplayMode
        };
        if (MyInvocation.BoundParameters.ContainsKey(nameof(MinLevel))) teamOptions.MinLevel = MinLevel;
        if (MyInvocation.BoundParameters.ContainsKey(nameof(MaxLevel))) teamOptions.MaxLevel = MaxLevel;
        chart.AddTeam(TeamId, TeamLabel, _members, teamOptions);

        var renderOptions = new TopologyRenderOptions {
            IncludeLegend = ShowLegend.IsPresent,
            NodeDisplayMode = NodeDisplayMode,
            CanvasSurfaceStyle = TopologyCanvasSurfaceStyle.Plain
        };
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) Directory.CreateDirectory(directory!);
        var extension = Path.GetExtension(output);
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) chart.SaveSvg(output, renderOptions);
        else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) chart.SaveHtml(output, renderOptions);
        else chart.SavePng(output, renderOptions);

        if (Show.IsPresent) ImagePlayground.Helpers.Open(output, true);
        if (PassThru.IsPresent) WriteObject(chart);
    }

    private void AddDefinitionResults(IEnumerable<PSObject> results) {
        foreach (var result in results) {
            var value = result.BaseObject;
            if (value is TopologyTeamMember member) {
                _members.Add(member);
            } else if (value != null) {
                var exception = new PSArgumentException("MemberDefinition must emit only New-ImageOrganizationMember results.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageOrganizationChartUnsupportedMember", ErrorCategory.InvalidArgument, value));
            }
        }
    }

    private void ValidateExtension(string extension, string output) {
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) return;
        var exception = new PSArgumentException("Organization chart output supports only .png, .svg, .html, or .htm file extensions.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageOrganizationChartUnsupportedExtension", ErrorCategory.InvalidArgument, output));
    }
}
