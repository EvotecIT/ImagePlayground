using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
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
