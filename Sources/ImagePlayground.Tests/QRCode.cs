using System;
using System.IO;
using System.Net;
using Xunit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using QRCoder;


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
    public void Test_QRCode_CustomColors() {

        string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeColors.png");
        File.Delete(filePath);
        QrCode.Generate("https://evotec.xyz", filePath, false, QRCodeGenerator.ECCLevel.Q, Color.Red, Color.Yellow, 10);

        Assert.True(File.Exists(filePath) == true);

        using SixLabors.ImageSharp.Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);
        Assert.Equal(Color.Yellow.ToPixel<Rgba32>(), img[0, 0]);

        var read = QrCode.Read(filePath);
        Assert.Equal("https://evotec.xyz", read.Message);
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

    [Theory]
    [InlineData("PNG")]
    [InlineData("JPG")]
    [InlineData("ICO")]
    public void Test_QRCodeUppercaseExtensions(string extension) {
        string filePath = System.IO.Path.Combine(_directoryWithImages, $"QRCodeUpper.{extension}");
        File.Delete(filePath);
        QrCode.Generate("https://evotec.xyz", filePath);
        Assert.True(File.Exists(filePath));
    }
      
    [Fact]
    public void Test_QRCode_WithLogo() {
        string filePath = Path.Combine(_directoryWithImages, "QRCodeWithLogo.png");
        string logoPath = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        File.Delete(filePath);
        Assert.True(File.Exists(filePath) == false);

        string basePath = Path.Combine(_directoryWithImages, "QRCodeBase.png");
        File.Delete(basePath);
        QrCode.Generate("https://evotec.xyz", basePath);
        byte[] baseBytes = File.ReadAllBytes(basePath);

        QrCode.Generate("https://evotec.xyz", filePath, logoPath);

        Assert.True(File.Exists(filePath) == true);

        var read = QrCode.Read(filePath);
        Assert.True(read.Message == "https://evotec.xyz");

        byte[] logoBytes = File.ReadAllBytes(filePath);
        Assert.NotEqual(baseBytes, logoBytes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Test_QRCode_InvalidPixelSize(int pixelSize) {
        string filePath = Path.Combine(_directoryWithImages, "QRCodeInvalid.png");
        Assert.Throws<ArgumentOutOfRangeException>(() => QrCode.Generate("https://evotec.xyz", filePath, false, QRCodeGenerator.ECCLevel.Q, null, null, pixelSize));
    }
}