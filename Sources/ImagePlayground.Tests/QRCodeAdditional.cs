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
}
