using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a simple grid-based image.</summary>
/// <example>
///   <summary>Create 100x100 grid</summary>
///   <code>New-ImageGrid -FilePath out.png -Width 100 -Height 100</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageGrid")]
public sealed class NewImageGridCmdlet : PSCmdlet {
    /// <summary>Output file path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Width of the new image.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    [ValidateRange(1, 1000)]
    public int Width { get; set; }

    /// <summary>Height of the new image.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    [ValidateRange(1, 1000)]
    public int Height { get; set; }

    /// <summary>Background color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color Color { get; set; } = SixLabors.ImageSharp.Color.White;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var output = Helpers.ResolvePath(FilePath);
        Helpers.CreateParentDirectory(output);
        ImagePlayground.ImageHelper.Create(output, Width, Height, Color, Open.IsPresent);
    }
}
