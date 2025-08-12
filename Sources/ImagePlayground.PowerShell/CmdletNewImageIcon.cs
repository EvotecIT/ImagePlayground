using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an icon file from an image.</summary>
/// <para>Generates multiple icon sizes using <see cref="Size"/>.</para>
/// <example>
///   <summary>Create 16x16 and 32x32 icons</summary>
///   <code>New-ImageIcon -FilePath in.png -OutputPath icon.ico -Size 16,32</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageIcon")]
public sealed class NewImageIconCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination icon file.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Icon sizes to include.</summary>
    /// <para>Defaults to common icon sizes when empty.</para>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int[] Size { get; set; } = Array.Empty<int>();

    /// <summary>Open the icon after saving.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"New-ImageIcon - File {FilePath} not found. Please check the path.");
            return;
        }

        var output = Helpers.ResolvePath(OutputPath);
        Helpers.CreateParentDirectory(output);
        using var img = ImagePlayground.Image.Load(filePath);
        img.SaveAsIcon(output, Size);
        if (Open.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}
