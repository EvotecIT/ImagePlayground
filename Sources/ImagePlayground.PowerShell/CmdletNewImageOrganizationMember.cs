using ChartForgeX.Topology;
using System.Collections;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one member for an organization or team chart.</summary>
/// <para>The returned ChartForgeX team member can be passed to <c>New-ImageOrganizationChart</c>.</para>
/// <example>
///   <summary>Create a manager</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageOrganizationMember -Id lead -Name 'Avery Stone' -Role 'Engineering Lead' -Status Healthy</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageOrganizationMember")]
[OutputType(typeof(TopologyTeamMember))]
public sealed class NewImageOrganizationMemberCmdlet : PSCmdlet {
    /// <summary>Stable member identifier.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Member display name.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional role or job title.</summary>
    [Parameter(Position = 2)]
    public string Role { get; set; } = string.Empty;

    /// <summary>Manager or parent member identifier.</summary>
    [Parameter]
    public string ParentId { get; set; } = string.Empty;

    /// <summary>Optional explicit hierarchy level.</summary>
    [Parameter]
    public int Level { get; set; }

    /// <summary>Member health or availability state.</summary>
    [Parameter]
    public TopologyHealthStatus Status { get; set; } = TopologyHealthStatus.Unknown;

    /// <summary>Optional ChartForgeX topology icon identifier.</summary>
    [Parameter]
    public string IconId { get; set; } = string.Empty;

    /// <summary>Optional host-readable metadata copied to the generated topology node.</summary>
    [Parameter]
    public Hashtable? Metadata { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var member = new TopologyTeamMember(Id, Name, string.IsNullOrWhiteSpace(Role) ? null : Role) {
            ParentId = string.IsNullOrWhiteSpace(ParentId) ? null : ParentId,
            Status = Status,
            IconId = string.IsNullOrWhiteSpace(IconId) ? null : IconId
        };
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Level))) member.Level = Level;
        if (Metadata != null) {
            foreach (DictionaryEntry entry in Metadata) {
                if (entry.Key == null) continue;
                member.Metadata[entry.Key.ToString() ?? string.Empty] = entry.Value?.ToString() ?? string.Empty;
            }
        }
        WriteObject(member);
    }
}
