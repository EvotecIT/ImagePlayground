using QRCoder;
using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_QRCode_Bookmark() {
        string file = Path.Combine(_directoryWithTests, "qr_bookmark.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateBookmark("https://example.com", "Example", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.Bookmark("https://example.com", "Example").ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Email() {
        string file = Path.Combine(_directoryWithTests, "qr_email.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateEmail(file, "test@example.com", "hi", "body");
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.Mail("test@example.com", "hi", "body", PayloadGenerator.Mail.MailEncoding.MAILTO).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_WhatsApp() {
        string file = Path.Combine(_directoryWithTests, "qr_wa.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateWhatsAppMessage("hello there", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.WhatsAppMessage("hello there").ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Url() {
        string file = Path.Combine(_directoryWithTests, "qr_url.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateUrl("https://example.com", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.Url("https://example.com").ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Mms() {
        string file = Path.Combine(_directoryWithTests, "qr_mms.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateMMS(file, "+1234567890", "subj");
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.MMS("+1234567890", "subj", PayloadGenerator.MMS.MMSEncoding.MMSTO).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_CalendarEvent() {
        string file = Path.Combine(_directoryWithTests, "qr_event.png");
        if (File.Exists(file)) File.Delete(file);
        DateTime from = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime to = from.AddHours(1);
        QrCode.GenerateCalendarEvent("Meeting", "Discuss", "Loc", from, to, file, false);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.CalendarEvent("Meeting", "Discuss", "Loc", from, to, false, PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Contact() {
        string file = Path.Combine(_directoryWithTests, "qr_contact.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateContact(file, PayloadGenerator.ContactData.ContactOutputType.VCard4, "John", "Doe");
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.ContactData(PayloadGenerator.ContactData.ContactOutputType.VCard4, "John", "Doe", null, null, null, null, null, null, null, null, null, null, null, null, null, null, PayloadGenerator.ContactData.AddressOrder.Default, null, null).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Sms() {
        string file = Path.Combine(_directoryWithTests, "qr_sms.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateSms("+1234567890", "Hello", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.SMS("+1234567890", "Hello", PayloadGenerator.SMS.SMSEncoding.SMS).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Geo() {
        string file = Path.Combine(_directoryWithTests, "qr_geo.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateGeoLocation("52.1", "21.0", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.Geolocation("52.1", "21.0", PayloadGenerator.Geolocation.GeolocationEncoding.GEO).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Girocode() {
        string file = Path.Combine(_directoryWithTests, "qr_giro.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateGirocode("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, "Invoice");
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.Girocode("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, "Invoice", PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, string.Empty, string.Empty, PayloadGenerator.Girocode.GirocodeVersion.Version1, PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Bitcoin() {
        string file = Path.Combine(_directoryWithTests, "qr_btc.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateBitcoinAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.Bitcoin, "1BoatSLRHtKNngkdXEeobR76b53LETtpyT", 0.01, null, null, file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.Bitcoin, "1BoatSLRHtKNngkdXEeobR76b53LETtpyT", 0.01, null, null).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Monero() {
        string file = Path.Combine(_directoryWithTests, "qr_monero.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateMoneroTransaction("44AFFq5kSiGBoZ", 1f, null, null, null, file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.MoneroTransaction("44AFFq5kSiGBoZ", 1f, null, null, null).ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Phone() {
        string file = Path.Combine(_directoryWithTests, "qr_phone.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GeneratePhoneNumber("+123456", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.PhoneNumber("+123456").ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }

    [Fact]
    public void Test_QRCode_Skype() {
        string file = Path.Combine(_directoryWithTests, "qr_skype.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateSkypeCall("echo123", file);
        Assert.True(File.Exists(file));
        var expected = new PayloadGenerator.SkypeCall("echo123").ToString();
        var read = QrCode.Read(file);
        Assert.Equal(expected, read.Message);
    }
}
