using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HeifImportRejectsMalformedUtf8WithoutPublishingPartialMetadata(bool inPlace) {
        string source = Path.Combine(_directoryWithTests, "heif-invalid-import-" + inPlace + ".heic");
        string metadataPath = Path.Combine(_directoryWithTests, "heif-invalid-import-" + inPlace + ".json");
        string output = inPlace ? source : Path.Combine(_directoryWithTests, "heif-invalid-import-output.heic");
        byte[] original = CreateMinimalHeifWithPrimaryImageExifAndXmp(320, 240, CreateExifPayload("Original"), "Original XMP");
        byte[] sentinel = Encoding.ASCII.GetBytes("Existing destination");
        File.WriteAllBytes(source, original);
        if (!inPlace) {
            File.WriteAllBytes(output, sentinel);
        }
        var metadata = new Dictionary<string, object?> {
            ["HorizontalResolution"] = 96,
            ["VerticalResolution"] = 96,
            ["ResolutionUnits"] = "PixelsPerInch",
            ["ExifProfile"] = CreateExifPayload("Replacement"),
            ["XmpProfile"] = new byte[] { 0xC3, 0x28 }
        };
        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.ImportMetadata(source, metadataPath, output));

        Assert.Equal(original, File.ReadAllBytes(source));
        Assert.Equal(inPlace ? original : sentinel, File.ReadAllBytes(output));
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(output)!, Path.GetFileName(output) + ".*.tmp"));
    }
}
