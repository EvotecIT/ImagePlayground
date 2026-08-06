using ChartForgeX;
using ChartForgeX.Composition;
using ChartForgeX.Primitives;
using ImagePlayground;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a fixed-size visual canvas for social images, wallpapers, report covers, and announcement cards.</summary>
/// <para>Layer commands emit native ChartForgeX layers; ImagePlayground only binds PowerShell inputs and selects the output format.</para>
/// <example>
///   <summary>Create a social preview</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageCanvas -Preset SocialPreview -Title 'ChartForgeX 1.3' -Backdrop TechHorizon -LayerDefinition { New-ImageCanvasText -X 72 -Y 72 -Width 1000 -Text 'ChartForgeX 1.3' -FontSize 58 -Color White -Emphasized; New-ImageCanvasInfoTile -X 72 -Y 190 -Width 360 -Height 150 -Icon SVG -Label 'Renderer' -Value 'Dependency-free' } -FilePath preview.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageCanvas", DefaultParameterSetName = DefinitionSet)]
[OutputType(typeof(VisualCanvas))]
public sealed class NewImageCanvasCmdlet : PSCmdlet {
    private const string DefinitionSet = "Definition";
    private const string LayerSet = "Layer";
    private const string CanvasSet = "Canvas";
    private readonly List<VisualCanvasLayer> _layers = new();
    private readonly List<VisualCanvas> _canvases = new();

    /// <summary>Script block that emits visual canvas layers.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = DefinitionSet)]
    public ScriptBlock? LayerDefinition { get; set; }

    /// <summary>Canvas layers supplied directly or through the pipeline.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = LayerSet)]
    public VisualCanvasLayer[] Layer { get; set; } = Array.Empty<VisualCanvasLayer>();

    /// <summary>Existing ChartForgeX visual canvas to render.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = CanvasSet)]
    public VisualCanvas? Canvas { get; set; }

    /// <summary>Built-in canvas size preset.</summary>
    [Parameter(ParameterSetName = DefinitionSet)]
    [Parameter(ParameterSetName = LayerSet)]
    public ImageCanvasPreset Preset { get; set; } = ImageCanvasPreset.Custom;

    /// <summary>Custom canvas width.</summary>
    [Parameter(ParameterSetName = DefinitionSet)]
    [Parameter(ParameterSetName = LayerSet)]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 1200;

    /// <summary>Custom canvas height.</summary>
    [Parameter(ParameterSetName = DefinitionSet)]
    [Parameter(ParameterSetName = LayerSet)]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 630;

    /// <summary>Accessibility title for SVG output.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Top or solid background color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? BackgroundTop { get; set; }

    /// <summary>Bottom background color for a vertical gradient.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? BackgroundBottom { get; set; }

    /// <summary>Built-in canvas backdrop.</summary>
    [Parameter]
    public VisualCanvasBackdropStyle Backdrop { get; set; } = VisualCanvasBackdropStyle.Plain;

    /// <summary>PNG output pixel multiplier.</summary>
    [Parameter]
    [ValidateRange(1, 4)]
    public int PngOutputScale { get; set; } = 1;

    /// <summary>Output image path.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the generated canvas.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the configured ChartForgeX canvas to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Layer.Length > 0) _layers.AddRange(Layer);
        if (Canvas != null) _canvases.Add(Canvas);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        ValidateExtension(Path.GetExtension(output), output);
        PowerShellPathResolver.ValidateFileDestination(output, nameof(FilePath), nameof(FilePath));
        ValidateCanvasInput();
        var canvas = _canvases.Count == 1 ? _canvases[0] : CreateCanvas();
        ApplyBackground(canvas);
        if (LayerDefinition != null) {
            foreach (var result in LayerDefinition.Invoke()) AddLayer(result);
        }
        foreach (var layer in _layers) canvas.AddLayer(layer);
        if (!string.IsNullOrWhiteSpace(Title)) canvas.WithTitle(Title);
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Backdrop))) canvas.WithBackdrop(Backdrop);
        if (MyInvocation.BoundParameters.ContainsKey(nameof(PngOutputScale))) canvas.WithPngOutputScale(PngOutputScale);
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) Directory.CreateDirectory(directory!);
        canvas.Save(output);
        if (Show.IsPresent) ImagePlayground.Helpers.Open(output, true);
        if (PassThru.IsPresent) WriteObject(canvas);
    }

    private void ValidateCanvasInput() {
        if (ParameterSetName != CanvasSet || _canvases.Count == 1) return;
        var message = _canvases.Count == 0
            ? "New-ImageCanvas requires one ChartForgeX VisualCanvas."
            : "New-ImageCanvas accepts one ChartForgeX VisualCanvas per output path.";
        var errorId = _canvases.Count == 0 ? "NewImageCanvasMissingCanvas" : "NewImageCanvasMultipleCanvases";
        ThrowTerminatingError(new ErrorRecord(new PSArgumentException(message), errorId, ErrorCategory.InvalidArgument, null));
    }

    private void ValidateExtension(string extension, string output) {
        switch (extension.ToLowerInvariant()) {
            case ".svg":
            case ".html":
            case ".htm":
            case ".png":
            case ".jpg":
            case ".jpeg":
            case ".bmp":
            case ".ppm":
            case ".tif":
            case ".tiff":
                return;
            default:
                var exception = new PSArgumentException("Visual canvas output supports .svg, .html, .htm, .png, .jpg, .jpeg, .bmp, .ppm, .tif, or .tiff file extensions.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageCanvasUnsupportedExtension", ErrorCategory.InvalidArgument, output));
                return;
        }
    }

    private VisualCanvas CreateCanvas() {
        switch (Preset) {
            case ImageCanvasPreset.SocialPreview:
                return VisualCanvas.CreateSocialPreview();
            case ImageCanvasPreset.DesktopWallpaper:
                return VisualCanvas.CreateDesktopWallpaper();
            default:
                return VisualCanvas.Create(Width, Height);
        }
    }

    private void AddLayer(object? input) {
        var value = input is PSObject psObject ? psObject.BaseObject : input;
        if (value is VisualCanvasLayer layer) _layers.Add(layer);
        else if (value != null) {
            var exception = new PSArgumentException("LayerDefinition must emit only ChartForgeX visual canvas layers.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageCanvasUnsupportedLayer", ErrorCategory.InvalidArgument, value));
        }
    }

    private void ApplyBackground(VisualCanvas canvas) {
        var top = ChartColorConverter.Convert(BackgroundTop);
        var bottom = ChartColorConverter.Convert(BackgroundBottom);
        if (top.HasValue && bottom.HasValue) canvas.WithBackground(top.Value, bottom.Value);
        else if (top.HasValue) canvas.WithBackground(top.Value);
        else if (bottom.HasValue) {
            var exception = new PSArgumentException("BackgroundBottom requires BackgroundTop.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageCanvasMissingBackgroundTop", ErrorCategory.InvalidArgument, null));
        }
    }
}
