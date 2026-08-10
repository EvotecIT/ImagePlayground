using System;
using System.Management.Automation;
using System.Text;
using ChartForgeX.Composition;
using ChartForgeX.Core;
using ChartForgeX.Stories;
using ChartForgeX.Topology;
using ChartForgeX.VisualArtifacts;
using ChartForgeX.VisualBlocks;

namespace ImagePlayground.PowerShell;

/// <summary>Converts a ChartForgeX chart, grid, canvas, story, diagram, table, sequence, flow, or visual block into a reusable visual artifact.</summary>
/// <para>The artifact is the portable handoff contract for static export, OfficeIMO document placement, and native editable Visio projection.</para>
/// <example>
///   <summary>Create an artifact from a topology</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$artifact = $topology | ConvertTo-ImageVisualArtifact -Id service-map -AccessibleDescription 'Gateway to database service path.'</code>
///   <para>Creates a reusable artifact that can be exported by ImagePlayground or placed by PSWriteOffice.</para>
/// </example>
[Cmdlet(VerbsData.ConvertTo, "ImageVisualArtifact")]
[OutputType(typeof(VisualArtifact))]
public sealed class ConvertToImageVisualArtifactCmdlet : PSCmdlet {
    /// <para>ChartForgeX model or existing visual artifact.</para>
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public object? InputObject { get; set; }

    /// <para>Stable artifact identifier. Existing artifact ids are preserved when omitted.</para>
    [Parameter]
    public string Id { get; set; } = string.Empty;

    /// <para>Optional artifact title override.</para>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <para>Optional artifact subtitle override.</para>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <para>Concise accessible name for the visual.</para>
    [Parameter]
    public string AccessibleName { get; set; } = string.Empty;

    /// <para>Longer accessible description for the visual.</para>
    [Parameter]
    public string AccessibleDescription { get; set; } = string.Empty;

    /// <para>Optional BCP 47 language tag for accessible text.</para>
    [Parameter]
    public string Language { get; set; } = string.Empty;

    /// <para>Marks the visual as decorative.</para>
    [Parameter]
    public SwitchParameter Decorative { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        object? value = InputObject is PSObject wrapped ? wrapped.BaseObject : InputObject;
        VisualArtifact artifact = value switch {
            VisualArtifact existing => existing,
            Chart chart => chart.ToVisualArtifact(OptionalId()),
            ChartGrid grid => grid.ToVisualArtifact(OptionalId()),
            VisualCanvas canvas => canvas.ToVisualArtifact(OptionalId()),
            VisualStory story => story.ToVisualArtifact(OptionalId()),
            TopologyChart topology => topology.ToVisualArtifact(),
            FlowArtifact flow => flow.ToVisualArtifact(),
            SequenceArtifact sequence => sequence.ToVisualArtifact(),
            TableArtifact table => table.ToVisualArtifact(),
            IVisualBlock block => block.ToVisualArtifact(OptionalId()),
            _ => throw new PSArgumentException("InputObject must be a ChartForgeX chart, grid, canvas, story, topology, flow, sequence, table, visual block, or VisualArtifact.", nameof(InputObject))
        };

        if (!string.IsNullOrWhiteSpace(Id)) {
            artifact.Id = Id.Trim();
        }
        if (!string.IsNullOrWhiteSpace(Title)) {
            artifact.Title = Title.Trim();
        }
        if (!string.IsNullOrWhiteSpace(Subtitle)) {
            artifact.Subtitle = Subtitle.Trim();
        }
        if (Decorative.IsPresent) {
            artifact.Accessibility.AsDecorative();
        } else if (!string.IsNullOrWhiteSpace(AccessibleName)) {
            artifact.Accessibility.WithTextAlternative(
                AccessibleName,
                string.IsNullOrWhiteSpace(AccessibleDescription) ? null : AccessibleDescription,
                string.IsNullOrWhiteSpace(Language) ? null : Language);
        } else {
            if (!string.IsNullOrWhiteSpace(AccessibleDescription)) {
                artifact.Accessibility.Description = AccessibleDescription;
            }
            if (!string.IsNullOrWhiteSpace(Language)) {
                artifact.Accessibility.Language = Language;
            }
        }
        PSObject output = InputObject is PSObject inputWrapper && ReferenceEquals(inputWrapper.BaseObject, artifact)
            ? inputWrapper
            : PSObject.AsPSObject(artifact);
        if (!output.TypeNames.Contains("ImagePlayground.VisualArtifact")) {
            output.TypeNames.Insert(0, "ImagePlayground.VisualArtifact");
        }

        SetPortableProperty(output, "OfficeVisualSvg", Encoding.UTF8.GetBytes(artifact.ToSvg()));
        SetPortableProperty(output, "OfficeVisualInterchangeJson", artifact.ToInterchangeUtf8Json());
        SetPortableProperty(output, "OfficeVisualInterchangeSchema", VisualArtifactInterchangeEnvelope.SchemaId);
        SetPortableProperty(output, "OfficeVisualInterchangeVersion", VisualArtifactInterchangeEnvelope.CurrentVersion);
        SetPortableProperty(output, "OfficeVisualKind", artifact.Kind.ToString());
        SetPortableProperty(output, "OfficeVisualId", artifact.Id);
        SetPortableProperty(output, "OfficeVisualTitle", artifact.Title);
        SetPortableProperty(
            output,
            "OfficeVisualAlternativeText",
            artifact.Accessibility.Description ?? artifact.Accessibility.Name ?? string.Empty);
        WriteObject(output);
    }

    private static void SetPortableProperty(PSObject output, string name, object? value) {
        PSPropertyInfo? existing = output.Properties[name];
        if (existing == null) {
            output.Properties.Add(new PSNoteProperty(name, value));
        } else {
            existing.Value = value;
        }
    }

    private string? OptionalId() => string.IsNullOrWhiteSpace(Id) ? null : Id.Trim();

}
