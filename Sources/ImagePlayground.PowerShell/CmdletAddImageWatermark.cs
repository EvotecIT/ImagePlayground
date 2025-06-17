using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground.PowerShell {
    [Cmdlet(VerbsCommon.Add, "ImageWatermark", DefaultParameterSetName = ParameterSetPlacement)]
    public sealed class AddImageWatermarkCmdlet : PSCmdlet {
        private const string ParameterSetPlacement = "Placement";
        private const string ParameterSetCoordinates = "Coordinates";

        [Parameter(Mandatory = true, Position = 0)]
        public string FilePath { get; set; } = string.Empty;

        [Parameter(Mandatory = true, Position = 1)]
        public string OutputPath { get; set; } = string.Empty;

        [Parameter(Mandatory = true, Position = 2)]
        public string WatermarkPath { get; set; } = string.Empty;

        [Parameter(ParameterSetName = ParameterSetPlacement)]
        public ImagePlayground.Image.WatermarkPlacement Placement { get; set; } = ImagePlayground.Image.WatermarkPlacement.Middle;

        [Parameter(ParameterSetName = ParameterSetCoordinates)]
        public int X { get; set; }

        [Parameter(ParameterSetName = ParameterSetCoordinates)]
        public int Y { get; set; }

        [Parameter]
        public float Opacity { get; set; } = 1f;

        [Parameter(ParameterSetName = ParameterSetPlacement)]
        public float Padding { get; set; } = 18f;

        [Parameter]
        public int Rotate { get; set; }

        [Parameter]
        public FlipMode FlipMode { get; set; } = FlipMode.None;

        [Parameter]
        public int WatermarkPercentage { get; set; } = 20;

        protected override void ProcessRecord() {
            string src = ImagePlayground.Helpers.ResolvePath(FilePath);
            string dest = ImagePlayground.Helpers.ResolvePath(OutputPath);
            string watermark = ImagePlayground.Helpers.ResolvePath(WatermarkPath);
            if (!File.Exists(src)) {
                WriteWarning($"Add-ImageWatermark - File {FilePath} not found. Please check the path.");
                return;
            }
            if (!File.Exists(watermark)) {
                WriteWarning($"Add-ImageWatermark - Watermark file {WatermarkPath} not found. Please check the path.");
                return;
            }

            if (ParameterSetName == ParameterSetCoordinates) {
                ImagePlayground.ImageHelper.WatermarkImage(src, dest, watermark, X, Y, Opacity, Rotate, FlipMode, WatermarkPercentage);
            } else {
                ImagePlayground.ImageHelper.WatermarkImage(src, dest, watermark, Placement, Opacity, Padding, Rotate, FlipMode, WatermarkPercentage);
            }
        }
    }
}
