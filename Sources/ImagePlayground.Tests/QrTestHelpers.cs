using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    private static void AssertQrDecoded(string filePath, string? expected = null) {
        var read = QrCode.Read(filePath);
        Assert.NotNull(read);
        if (expected is not null) {
            Assert.Equal(expected, read.Text);
        }
    }
}
