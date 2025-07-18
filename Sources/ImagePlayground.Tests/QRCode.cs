using System.IO;
using System.Net;
using Xunit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace ImagePlayground.Tests;

/// <summary>
/// Tests for QRCode.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_QRCodeUrl() {

        string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrl.jpg");
        File.Delete(filePath);
        Assert.True(File.Exists(filePath) == false);

        QrCode.Generate("https://evotec.xyz", filePath);

        Assert.True(File.Exists(filePath) == true);

        var read = QrCode.Read(filePath);
        Assert.True(read.Message == "https://evotec.xyz");
    }

    [Fact]
    public void Test_QRCodeWiFi() {
        string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeWiFi.png");
        File.Delete(filePath);
        Assert.True(File.Exists(filePath) == false);

        QrCode.GenerateWiFi("Evotec", "superHardPassword123!", filePath, true);

        Assert.True(File.Exists(filePath) == true);

        var read = QrCode.Read(filePath);
        Assert.True(read.Message == "WIFI:T:WPA;S:Evotec;P:superHardPassword123!;;");
    }

    [Fact]
    public void Test_QRCode_Transparent() {
        string filePath = Path.Combine(_directoryWithImages, "QRCodeTransparent.png");
        File.Delete(filePath);
        QrCode.Generate("https://evotec.xyz", filePath, true);
        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> img = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(filePath);
        Assert.Equal(0, img[0, 0].A);
    }

    [Fact]
    public void Test_BarCode() {
        string filePath = System.IO.Path.Combine(_directoryWithTests, "BarcodeEAN13.png");
        BarCode.Generate(BarcodeType.EAN, "9012341234571", filePath);

        var read1 = BarCode.Read(filePath);
        Assert.True(read1.Message == "9012341234571");
        Assert.True(File.Exists(filePath) == true);

        filePath = System.IO.Path.Combine(_directoryWithTests, "BarcodeEAN7.png");
        BarCode.Generate(BarcodeType.EAN, "96385074", filePath);
        Assert.True(File.Exists(filePath) == true);

        var read2 = BarCode.Read(filePath);
        Assert.True(read2.Message == "96385074");
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("pass123")]
    [InlineData("pass;123!")]
    public void Test_QRCodeWiFi_Passwords(string password) {
        string filePath = System.IO.Path.Combine(_directoryWithImages, $"WiFi_{password}.png");
        File.Delete(filePath);
        Assert.True(File.Exists(filePath) == false);

        QrCode.GenerateWiFi("TestSSID", password, filePath, true);

        Assert.True(File.Exists(filePath) == true);

        var read = QrCode.Read(filePath);
        string expected = $"WIFI:T:WPA;S:TestSSID;P:{WebUtility.UrlEncode(password)};;";
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCodeIcon() {
        string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrl.ico");
        File.Delete(filePath);
        Assert.True(File.Exists(filePath) == false);

        QrCode.Generate("https://evotec.xyz", filePath);

        Assert.True(File.Exists(filePath) == true);
    }
}