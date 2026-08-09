using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using ChartForgeX.Core;
using ChartForgeX.Topology;
using ChartForgeX.VisualArtifacts;

namespace ImagePlayground.PowerShell;

/// <summary>Exports a ChartForgeX visual artifact to static SVG, HTML, or PNG output.</summary>
/// <para>Applies shared watermarks, PNG DPI metadata, topology layout presets, and optional layout diagnostics without changing the source artifact.</para>
/// <example>
///   <summary>Export a watermarked 144 DPI PNG</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$artifact | Export-ImageVisualArtifact -FilePath report.png -Watermark $mark -Dpi 144</code>
///   <para>Exports the artifact through the shared ChartForgeX static rendering pipeline.</para>
/// </example>
[Cmdlet(VerbsData.Export, "ImageVisualArtifact")]
[OutputType(typeof(VisualArtifact))]
public sealed class ExportImageVisualArtifactCmdlet : ImageCmdlet {
    private readonly List<VisualArtifact> _artifacts = new();

    /// <para>Visual artifact to export.</para>
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public VisualArtifact? Artifact { get; set; }

    /// <para>Destination .svg, .html, .htm, or .png path.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <para>Watermarks applied in declaration order.</para>
    [Parameter]
    public VisualWatermark[] Watermark { get; set; } = Array.Empty<VisualWatermark>();

    /// <para>PNG physical resolution metadata in dots per inch.</para>
    [Parameter]
    [ValidateRange(1D, 100000D)]
    public double Dpi { get; set; } = 96D;

    /// <para>Reusable topology spacing and presentation profile.</para>
    [Parameter]
    public TopologyLayoutPreset TopologyLayoutPreset { get; set; } = TopologyLayoutPreset.Automatic;

    /// <para>Draw a developer-oriented topology layout diagnostic overlay.</para>
    [Parameter]
    public SwitchParameter IncludeTopologyDiagnostics { get; set; }

    /// <para>Open the generated artifact after creation.</para>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <para>Write the source artifact to the pipeline after export.</para>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Artifact == null) {
            throw new PSArgumentNullException(nameof(Artifact));
        }

        _artifacts.Add(Artifact);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        if (_artifacts.Count != 1) {
            throw new PSArgumentException(
                "Export-ImageVisualArtifact accepts exactly one artifact per output path. Invoke it once per artifact to avoid overwriting output.",
                nameof(Artifact));
        }

        VisualArtifact artifact = _artifacts[0];
        string output = Helpers.ResolvePath(FilePath);
        string extension = Path.GetExtension(output);
        var options = new VisualArtifactRenderOptions();
        foreach (VisualWatermark watermark in Watermark) {
            if (watermark == null) {
                throw new PSArgumentException("Watermark cannot contain null entries.", nameof(Watermark));
            }

            options.Watermarks.Add(watermark);
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Dpi))) {
            options.Raster = new RasterImageOptions { Dpi = Dpi };
        }
        if (TopologyLayoutPreset != TopologyLayoutPreset.Automatic || IncludeTopologyDiagnostics.IsPresent) {
            options.Topology = new TopologyRenderOptions {
                LayoutPreset = TopologyLayoutPreset,
                IncludeLayoutDiagnosticOverlay = IncludeTopologyDiagnostics.IsPresent
            };
        }
        EnsureDirectory(output);
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            artifact.SaveSvg(output, options);
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            artifact.SaveHtml(output, options);
        } else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            artifact.SavePng(output, options);
        } else {
            throw new PSArgumentException("Visual artifact output supports only .svg, .html, .htm, or .png file extensions.", nameof(FilePath));
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }

        if (PassThru.IsPresent) {
            WriteObject(artifact);
        }
    }

    private static void EnsureDirectory(string output) {
        string? directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }
    }
}
