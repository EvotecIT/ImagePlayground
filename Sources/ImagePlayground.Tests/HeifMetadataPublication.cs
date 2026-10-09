using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void HeifCombinedImportClearsSelectedProfilesMissingFromReplacementMetadata() {
        string input = Path.Combine(_directoryWithTests, "heif-import-empty-profiles-input.heic");
        string output = Path.Combine(_directoryWithTests, "heif-import-empty-profiles-output.heic");
        string metadataPath = Path.Combine(_directoryWithTests, "heif-import-empty-profiles.json");
        File.WriteAllBytes(input, CreateMinimalHeifWithPrimaryImageExifAndXmp(320, 240, CreateExifPayload("Original"), "<x:xmpmeta>Original metadata</x:xmpmeta>"));
        WriteHeifMetadataJson(input, metadataPath, null, null);

        global::ImagePlayground.ImageHelper.ImportMetadata(input, metadataPath, output);

        Assert.Empty(PlaygroundImage.GetExifValues(output));
        Assert.Equal(string.Empty, PlaygroundImage.GetHeifXmp(output));
        Assert.Equal("Original", Assert.Single(PlaygroundImage.GetExifValues(input), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
    }

    [Theory]
    [InlineData("invalid-utf8")]
    [InlineData("unwritable-xmp")]
    [InlineData("missing-xmp")]
    public void HeifCombinedImportRejectsInvalidSelectedProfilesWithoutChangingEitherFile(string failure) {
        string root = Path.Combine(_directoryWithTests, "heif-combined-failure-" + failure);
        Directory.CreateDirectory(root);
        string input = Path.Combine(root, "input.heic");
        string output = Path.Combine(root, "output.heic");
        string metadataPath = Path.Combine(root, "replacement.json");
        byte[] original = failure == "missing-xmp"
            ? CreateMinimalHeifWithPrimaryImageAndExif(320, 240, CreateExifPayload("Original"))
            : CreateHeifMetadataSiblings(CreateExifPayload("Original"), "<x:xmpmeta>Preserved XMP</x:xmpmeta>", 0, failure == "unwritable-xmp" ? 1 : 0);
        byte[] sentinel = Encoding.ASCII.GetBytes("Existing destination remains unchanged");
        File.WriteAllBytes(input, original);
        File.WriteAllBytes(output, sentinel);
        var metadata = new Dictionary<string, object?> {
            ["HorizontalResolution"] = 96,
            ["VerticalResolution"] = 96,
            ["ResolutionUnits"] = "PixelsPerInch",
            ["ExifProfile"] = CreateExifPayload("Replacement"),
            ["XmpProfile"] = failure == "invalid-utf8" ? new byte[] { 0xC3, 0x28 } : Encoding.UTF8.GetBytes("<x:xmpmeta>Replacement XMP</x:xmpmeta>")
        };
        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.ImportMetadata(input, metadataPath, output));

        Assert.Equal(original, File.ReadAllBytes(input));
        Assert.Equal(sentinel, File.ReadAllBytes(output));
        Assert.Equal(new[] { "input.heic", "output.heic", "replacement.json" }, Directory.GetFiles(root).Select(Path.GetFileName).OrderBy(name => name));
    }

    [Fact]
    public void HeifRemovalOfAnAbsentSelectedProfileCopiesTheOriginalContainer() {
        string input = Path.Combine(_directoryWithTests, "heif-absent-xmp-input.heic");
        string output = Path.Combine(_directoryWithTests, "heif-absent-xmp-output.heic");
        byte[] original = CreateMinimalHeifWithPrimaryImageAndExif(320, 240, CreateExifPayload("Preserved EXIF"));
        File.WriteAllBytes(input, original);

        var result = global::ImagePlayground.ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(input, output) { MetadataTypes = ImageMetadataType.Xmp });

        Assert.Equal(ImageMetadataType.None, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
        Assert.Equal(original, File.ReadAllBytes(input));
        Assert.Equal(original, File.ReadAllBytes(output));
        Assert.Equal("Preserved EXIF", Assert.Single(PlaygroundImage.GetExifValues(output), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
    }

#if NET8_0
    [Theory]
    [InlineData(ImageMetadataType.Exif)]
    [InlineData(ImageMetadataType.Xmp)]
    [InlineData(ImageMetadataType.All)]
    public void HeifPublicMetadataCommandsImportThenRemoveOnlySelectedFamilies(ImageMetadataType selected) {
        using var fixture = new PowerShellPathFixture();
        string input = Path.Combine(fixture.Location, "source.heic");
        string imported = Path.Combine(fixture.Location, "imported.heic");
        string output = Path.Combine(fixture.Location, "clean.heic");
        string metadataPath = Path.Combine(fixture.Location, "replacement.json");
        const string xmp = "<x:xmpmeta>Imported metadata</x:xmpmeta>";
        File.WriteAllBytes(input, CreateMinimalHeifWithPrimaryImageExifAndXmp(320, 240, CreateExifPayload("Original"), "<x:xmpmeta>Original metadata</x:xmpmeta>"));
        WriteHeifMetadataJson(input, metadataPath, CreateExifPayload("Imported"), xmp);

        fixture.Invoke("Import-ImageMetadata", ("FilePath", "./source.heic"), ("MetadataPath", "./replacement.json"), ("OutputPath", "./imported.heic"));
        Assert.Equal("Imported", Assert.Single(PlaygroundImage.GetExifValues(imported), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
        Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(imported));
        var result = Assert.IsType<ImageMetadataRemovalResult>(fixture.Invoke("Remove-ImageMetadata", ("FilePath", "./imported.heic"), ("OutputPath", "./clean.heic"), ("MetadataType", new[] { selected }), ("PassThru", true)).Single().BaseObject);

        Assert.Equal(selected == ImageMetadataType.All ? ImageMetadataType.Exif | ImageMetadataType.Xmp : selected, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
        bool clearsExif = (selected & ImageMetadataType.Exif) != 0;
        bool clearsXmp = (selected & ImageMetadataType.Xmp) != 0;
        Assert.Equal(clearsExif, PlaygroundImage.GetExifValues(output).Count == 0);
        Assert.Equal(clearsXmp ? string.Empty : xmp, PlaygroundImage.GetHeifXmp(output));
        Assert.Equal(320U, PlaygroundImage.GetHeifInfo(output).Width);
        Assert.Equal(240U, PlaygroundImage.GetHeifInfo(output).Height);
    }
#endif
}
