using System.IO;
using ChartForgeX;
using ChartForgeX.Core;
using CodeGlyphX.Payloads;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void Test_NativeChartForgeXChartRendersWithoutImagePlaygroundModels() {
        var file = Path.Combine(_directoryWithTests, "chart-native.png");
        if (File.Exists(file)) File.Delete(file);

        Chart.Create()
            .WithSize(360, 220)
            .WithTitle("Native ChartForgeX")
            .WithGrid()
            .AddBar("A", ChartPoints.FromValues(1, 2, 3))
            .AddBar("B", ChartPoints.FromValues(3, 4, 5))
            .Save(file);

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        Assert.True(stream.Length > 64);
    }

    [Fact]
    public void Test_GenerateContactQr() {
        var file = Path.Combine(_directoryWithTests, "contact.png");
        if (File.Exists(file)) File.Delete(file);

        QrCode.GenerateContact(file, QrContactOutputType.VCard4, "John", "Doe");

        Assert.True(File.Exists(file));
        var read = QrCode.Read(file);
        Assert.NotNull(read);
        Assert.Contains("BEGIN", read.Text);
    }
}
