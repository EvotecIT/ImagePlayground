using ImagePlayground;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground.PowerShell;

/// <summary>Sets image rotation.</summary>
/// <para>Use <see cref="Degrees"/> for arbitrary angles or <see cref="RotateMode"/> for predefined rotations.</para>
/// <example>
///   <summary>Rotate by 90 degrees</summary>
///   <code>Set-ImageRotation -FilePath in.png -OutputPath out.png -Degrees 90</code>
/// </example>
/// <example>
///   <summary>Rotate using RotateMode</summary>
///   <code>Set-ImageRotation -FilePath in.png -OutputPath out.png -RotateMode Rotate180</code>
/// </example>
[Cmdlet(VerbsCommon.Set, "ImageRotation", DefaultParameterSetName = ParameterSetDegrees)]
public sealed class SetImageRotationCmdlet : PSCmdlet {
        private const string ParameterSetDegrees = "Degrees";
        private const string ParameterSetMode = "Mode";

        /// <summary>Path to the source image.</summary>
        /// <para>The image must exist.</para>
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetDegrees)]
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetMode)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination file path.</summary>
        /// <para>Supported formats depend on the file extension.</para>
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetDegrees)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetMode)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>Angle in degrees.</summary>
        /// <para>Use for arbitrary rotations.</para>
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetDegrees)]
        public float Degrees { get; set; }

        /// <summary>Predefined rotation mode.</summary>
        /// <para>Use when rotating 90, 180 or 270 degrees.</para>
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetMode)]
        public RotateMode RotateMode { get; set; }

        /// <summary>Use asynchronous processing.</summary>
        [Parameter]
        public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Set-ImageRotation - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);
        Helpers.CreateParentDirectory(output);

        if (ParameterSetName == ParameterSetDegrees) {
            if (Async.IsPresent) {
                ImagePlayground.ImageHelper.RotateAsync(filePath, output, Degrees).GetAwaiter().GetResult();
            } else {
                ImagePlayground.ImageHelper.Rotate(filePath, output, Degrees);
            }
        } else {
            if (Async.IsPresent) {
                ImagePlayground.ImageHelper.RotateAsync(filePath, output, RotateMode).GetAwaiter().GetResult();
            } else {
                ImagePlayground.ImageHelper.Rotate(filePath, output, RotateMode);
            }
        }
    }
}
