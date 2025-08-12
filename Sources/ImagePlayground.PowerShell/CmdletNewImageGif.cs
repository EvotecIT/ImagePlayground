using ImagePlayground;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an animated GIF from existing images.</summary>
/// <example>
///   <summary>Build a GIF from two images</summary>
///   <code>New-ImageGif -Frames img1.png,img2.png -FilePath out.gif -FrameDelay 100</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageGif")]
public sealed class NewImageGifCmdlet : PSCmdlet {
    /// <summary>Source image paths used as frames.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string[] Frames { get; set; } = Array.Empty<string>();

    /// <summary>Output path for the GIF animation.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Delay between frames in milliseconds.</summary>
    [Parameter]
    public int FrameDelay { get; set; } = 100;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var checkedFrames = new List<string>();
        foreach (var frame in Frames) {
            var full = Helpers.ResolvePath(frame);
            if (!File.Exists(full)) {
                WriteWarning($"New-ImageGif - File {frame} not found. Please check the path.");
                return;
            }
            checkedFrames.Add(full);
        }

        var output = Helpers.ResolvePath(FilePath);
        Helpers.CreateParentDirectory(output);
        Gif.Generate(checkedFrames, output, FrameDelay);
    }
}

