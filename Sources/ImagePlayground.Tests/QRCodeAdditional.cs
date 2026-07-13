using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using CodeGlyphX;
using CodeGlyphX.Payloads;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for QRCodeAdditional.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_QRCode_Bookmark() {
        string file = Path.Combine(_directoryWithTests, "qr_bookmark.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateBookmark("https://example.com", "Example", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Bookmark("https://example.com", "Example").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Email() {
        string file = Path.Combine(_directoryWithTests, "qr_email.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateEmail(file, "test@example.com", "hi", "body");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Email("test@example.com", "hi", "body", QrMailEncoding.Mailto).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_WhatsApp() {
        string file = Path.Combine(_directoryWithTests, "qr_wa.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateWhatsAppMessage("hello there", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.WhatsAppMessage("hello there").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Url() {
        string file = Path.Combine(_directoryWithTests, "qr_url.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateUrl("https://example.com", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Url("https://example.com").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Mms() {
        string file = Path.Combine(_directoryWithTests, "qr_mms.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateMMS(file, "+1234567890", "subj");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Mms("+1234567890", "subj", QrMmsEncoding.Mmsto).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_CalendarEvent() {
        string file = Path.Combine(_directoryWithTests, "qr_event.png");
        if (File.Exists(file)) File.Delete(file);
        DateTime from = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime to = from.AddHours(1);
        QrCode.GenerateCalendarEvent("Meeting", "Discuss", "Loc", from, to, file, false);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.CalendarEvent("Meeting", "Discuss", "Loc", from, to, false, QrCalendarEncoding.ICalComplete).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Contact() {
        string file = Path.Combine(_directoryWithTests, "qr_contact.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateContact(file, QrContactOutputType.VCard4, "John", "Doe");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Contact(QrContactOutputType.VCard4, "John", "Doe").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Sms() {
        string file = Path.Combine(_directoryWithTests, "qr_sms.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateSms("+1234567890", "Hello", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Sms("+1234567890", "Hello", QrSmsEncoding.Sms).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Geo() {
        string file = Path.Combine(_directoryWithTests, "qr_geo.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateGeoLocation("52.1", "21.0", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Geo("52.1", "21.0", QrGeolocationEncoding.Geo).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Girocode() {
        string file = Path.Combine(_directoryWithTests, "qr_giro.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateGirocode("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, file, "Invoice");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Girocode("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, "Invoice", QrGirocodeRemittanceType.Unstructured, string.Empty, string.Empty, QrGirocodeVersion.Version1, QrGirocodeEncoding.Iso8859_1).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Bitcoin() {
        string file = Path.Combine(_directoryWithTests, "qr_btc.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateBitcoinAddress(QrBitcoinLikeType.Bitcoin, "1BoatSLRHtKNngkdXEeobR76b53LETtpyT", 0.01, null, null, file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.BitcoinLike(QrBitcoinLikeType.Bitcoin, "1BoatSLRHtKNngkdXEeobR76b53LETtpyT", 0.01, null, null).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Monero() {
        string file = Path.Combine(_directoryWithTests, "qr_monero.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateMoneroTransaction("44AFFq5kSiGBoZ", 1f, null, null, null, file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Monero("44AFFq5kSiGBoZ", 1f, null, null, null).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Phone() {
        string file = Path.Combine(_directoryWithTests, "qr_phone.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GeneratePhoneNumber("+123456", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Phone("+123456").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public void Test_QRCode_Skype() {
        string file = Path.Combine(_directoryWithTests, "qr_skype.png");
        if (File.Exists(file)) File.Delete(file);
        QrCode.GenerateSkypeCall("echo123", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.SkypeCall("echo123").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_WhatsAppAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_wa_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateWhatsAppMessageAsync("hello there", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.WhatsAppMessage("hello there").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_BookmarkAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_bookmark_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateBookmarkAsync("https://example.com", "Example", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Bookmark("https://example.com", "Example").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_MmsAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_mms_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateMMSAsync(file, "+1234567890", "subj");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Mms("+1234567890", "subj", QrMmsEncoding.Mmsto).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_GirocodeAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_giro_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateGirocodeAsync("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, file, "Invoice");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Girocode("DE12500105170648489890", "COBADEFFXXX", "Test", 1m, "Invoice", QrGirocodeRemittanceType.Unstructured, string.Empty, string.Empty, QrGirocodeVersion.Version1, QrGirocodeEncoding.Iso8859_1).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_MoneroAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_monero_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateMoneroTransactionAsync("44AFFq5kSiGBoZ", 1f, null, null, null, file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Monero("44AFFq5kSiGBoZ", 1f, null, null, null).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_PhoneAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_phone_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GeneratePhoneNumberAsync("+123456", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.Phone("+123456").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_SkypeAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_skype_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateSkypeCallAsync("echo123", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.SkypeCall("echo123").Text;
        AssertQrDecoded(file, expected);
    }
}
