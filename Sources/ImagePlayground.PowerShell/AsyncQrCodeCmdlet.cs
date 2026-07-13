using System;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Base class for QR-code cmdlets with shared output-path and preview helpers.
/// </summary>
public abstract class AsyncQrCodeCmdlet : AsyncImageCmdlet {
    /// <summary>
    /// Validates QR pixel size.
    /// </summary>
    /// <param name="pixelSize">Pixel size specified by the caller.</param>
    protected static void ValidatePixelSize(int pixelSize) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }
    }

    /// <summary>
    /// Ensures an output path exists, creating a temporary path when none was supplied.
    /// </summary>
    /// <param name="filePath">Output path provided by the caller.</param>
    /// <returns>Output path to use for QR generation.</returns>
    protected string EnsureQrOutputPath(string filePath) {
        if (!string.IsNullOrWhiteSpace(filePath)) {
            return filePath;
        }

        string resolvedPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
        WriteWarning($"{MyInvocation.MyCommand.Name} - No file path specified, saving to {resolvedPath}");
        return resolvedPath;
    }

    /// <summary>
    /// Opens the generated output when the caller requested preview.
    /// </summary>
    /// <param name="filePath">Generated file path.</param>
    /// <param name="show">Flag indicating whether preview was requested.</param>
    protected static void ShowGeneratedQrCode(string filePath, SwitchParameter show) {
        if (show.IsPresent) {
            Helpers.Open(Helpers.ResolvePath(filePath), true);
        }
    }
}
