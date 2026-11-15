using OfficeIMO.Drawing;
using Color = OfficeIMO.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.OfficeColor;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for HelpersColor.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ToHexColor_RemovesAlpha() {
        Color color = OfficeColor.ParseHex("11223344");
        string result = Helpers.ToHexColor(color);
        Assert.Equal("112233", result);
    }
}
