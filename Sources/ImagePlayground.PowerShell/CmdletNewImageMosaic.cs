using ImagePlayground;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a mosaic image from multiple files.</summary>
/// <example>
///   <summary>Create a 2x2 mosaic</summary>
///   <code>New-ImageMosaic -FilePaths img1.png,img2.png,img3.png,img4.png -OutputPath out.png -Columns 2 -Width 100 -Height 100</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageMosaic")]
public sealed class NewImageMosaicCmdlet : PSCmdlet {
    /// <summary>Source image paths.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string[] FilePaths { get; set; } = Array.Empty<string>();

    /// <summary>Output file path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Number of columns in the mosaic.</summary>
    [Parameter(Mandatory = true)]
    public int Columns { get; set; }

    /// <summary>Width of each tile.</summary>
    [Parameter(Mandatory = true)]
    public int Width { get; set; }

    /// <summary>Height of each tile.</summary>
    [Parameter(Mandatory = true)]
    public int Height { get; set; }

    /// <summary>Open the mosaic after creation.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var checkedFiles = new List<string>();
        foreach (var file in FilePaths) {
            var full = Helpers.ResolvePath(file);
            if (!File.Exists(full)) {
                WriteWarning($"New-ImageMosaic - File {file} not found. Please check the path.");
                return;
            }
            checkedFiles.Add(full);
        }
        var output = Helpers.ResolvePath(OutputPath);
        ImageHelper.Mosaic(checkedFiles, output, Columns, Width, Height);
        if (Open.IsPresent) {
            Helpers.Open(output, true);
        }
    }
}
