using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Theory]
    [InlineData(10, 30)]
    [InlineData(30, 10)]
    [InlineData(5, 5)]
    public void Test_CreateGridImage_InvalidDimensions_Throws(int width, int height) {
        string dest = Path.Combine(_directoryWithTests, $"invalid_{width}_{height}.png");
        Assert.Throws<ArgumentOutOfRangeException>(() => ImageHelper.Create(dest, width, height, SixLabors.ImageSharp.Color.White));
    }
}
