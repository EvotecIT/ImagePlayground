using ImagePlayground;

namespace ImagePlayground.PowerShell;
/// <summary>Adjusts image properties.</summary>
/// <para>Applies optional brightness, contrast, lightness, opacity, saturation or sepia adjustments.</para>
/// <example>
///   <summary>Increase brightness and contrast</summary>
///   <code>Set-ImageAdjust -FilePath in.png -OutputPath out.png -Brightness 1.2 -Contrast 1.1</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageAdjust")]
public sealed class SetImageAdjustCmdlet : PSCmdlet {
        /// <summary>Path to the source image.</summary>
        /// <para>The image must exist.</para>
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination file path.</summary>
        /// <para>Supported formats depend on the file extension.</para>
        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>Brightness adjustment factor.</summary>
        [Parameter]
        public float? Brightness { get; set; }

        /// <summary>Contrast adjustment factor.</summary>
        [Parameter]
        public float? Contrast { get; set; }

        /// <summary>Lightness adjustment factor.</summary>
        [Parameter]
        public float? Lightness { get; set; }

        /// <summary>Opacity adjustment factor.</summary>
        [Parameter]
        public float? Opacity { get; set; }

        /// <summary>Saturation adjustment factor.</summary>
        [Parameter]
        public float? Saturation { get; set; }

        /// <summary>Sepia adjustment factor.</summary>
        [Parameter]
        public float? Sepia { get; set; }

        /// <summary>Use asynchronous processing.</summary>
        [Parameter]
        public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageAdjust - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        if (Async.IsPresent) {
            ImageHelper.AdjustAsync(filePath, output, Brightness, Contrast, Lightness, Opacity, Saturation, Sepia).GetAwaiter().GetResult();
        } else {
            ImageHelper.Adjust(filePath, output, Brightness, Contrast, Lightness, Opacity, Saturation, Sepia);
        }
    }
}