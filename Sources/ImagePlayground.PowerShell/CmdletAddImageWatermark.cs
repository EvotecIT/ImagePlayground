using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground.PowerShell {
    /// <summary>Adds a watermark image to another image.</summary>
    /// <example>
    ///   <summary>Watermark a photo</summary>
    ///   <code>Add-ImageWatermark -FilePath photo.png -OutputPath out.png -WatermarkPath logo.png -Placement BottomRight</code>
    /// </example>
    [Cmdlet(VerbsCommon.Add, "ImageWatermark", DefaultParameterSetName = ParameterSetPlacement)]
    public sealed class AddImageWatermarkCmdlet : PSCmdlet {
        private const string ParameterSetPlacement = "Placement";
        private const string ParameterSetCoordinates = "Coordinates";

        /// <summary>Path to the source image.</summary>
        [Parameter(Mandatory = true, Position = 0)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>Destination path for the watermarked image.</summary>
        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>Image used as the watermark.</summary>
        [Parameter(Mandatory = true, Position = 2)]
        public string WatermarkPath { get; set; } = string.Empty;

        /// <summary>Watermark placement when coordinates are not specified.</summary>
        [Parameter(ParameterSetName = ParameterSetPlacement)]
        public ImagePlayground.Image.WatermarkPlacement Placement { get; set; } = ImagePlayground.Image.WatermarkPlacement.Middle;

        /// <summary>X coordinate for custom placement.</summary>
        [Parameter(ParameterSetName = ParameterSetCoordinates)]
        public int X { get; set; }

        /// <summary>Y coordinate for custom placement.</summary>
        [Parameter(ParameterSetName = ParameterSetCoordinates)]
        public int Y { get; set; }

        /// <summary>Opacity of the watermark.</summary>
        [Parameter]
        public float Opacity { get; set; } = 1f;

        /// <summary>Padding around the watermark.</summary>
        [Parameter(ParameterSetName = ParameterSetPlacement)]
        public float Padding { get; set; } = 18f;

        /// <summary>Rotation angle in degrees.</summary>
        [Parameter]
        public int Rotate { get; set; }

        /// <summary>Flip mode for the image.</summary>
        [Parameter]
        public FlipMode FlipMode { get; set; } = FlipMode.None;

        /// <summary>Scale of the watermark relative to the image.</summary>
        [Parameter]
        public int WatermarkPercentage { get; set; } = 20;

        /// <summary>Tile watermark across the image with given spacing.</summary>
        [Parameter]
        public int? Spacing { get; set; }

        /// <inheritdoc />
        protected override void ProcessRecord() {
            var filePath = Helpers.ResolvePath(FilePath);
            if (!File.Exists(filePath)) {
                WriteWarning($"Add-ImageWatermark - File {FilePath} not found. Please check the path.");
                return;
            }
            var watermark = Helpers.ResolvePath(WatermarkPath);
            if (!File.Exists(watermark)) {
                WriteWarning($"Add-ImageWatermark - Watermark file {WatermarkPath} not found. Please check the path.");
                return;
            }
            var output = Helpers.ResolvePath(OutputPath);

            if (Spacing != null) {
                ImagePlayground.ImageHelper.WatermarkImageTiled(filePath, output, watermark, Spacing.Value, Opacity, Rotate, FlipMode, WatermarkPercentage);
            } else if (ParameterSetName == ParameterSetCoordinates) {
                ImagePlayground.ImageHelper.WatermarkImage(filePath, output, watermark, X, Y, Opacity, Rotate, FlipMode, WatermarkPercentage);
            } else {
                ImagePlayground.ImageHelper.WatermarkImage(filePath, output, watermark, Placement, Opacity, Padding, Rotate, FlipMode, WatermarkPercentage);
            }
        }
    }
}
