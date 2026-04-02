using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    private static void AssertQrDecoded(string filePath, string? expected = null) {
        var read = QrCode.Read(filePath);
        Assert.Equal(Status.Found, read.Status);
        if (expected is not null) {
            Assert.Equal(expected, read.Message);
        }
    }
}
