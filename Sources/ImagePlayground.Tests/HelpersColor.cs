using SixLabors.ImageSharp;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for HelpersColor.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ToHexColor_RemovesAlpha() {
        Color color = Color.ParseHex("11223344");
        string result = Helpers.ToHexColor(color);
        Assert.Equal("112233", result);
    }
}