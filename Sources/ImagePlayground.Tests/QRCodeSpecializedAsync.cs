using System.IO;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Payloads;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for specialized async QR code generators.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public async Task Test_QRCode_ShadowSocksAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_shadowsocks_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateShadowSocksAsync("example.com", 8388, "secret", QrShadowSocksMethod.Aes256Gcm, file, "Warsaw");
        Assert.True(File.Exists(file));
        var expected = QrPayloads.ShadowSocks("example.com", 8388, "secret", QrShadowSocksMethod.Aes256Gcm, "Warsaw").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_OneTimePasswordAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_otp_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateOneTimePasswordAsync(OtpAuthType.Totp, "JBSWY3DPEHPK3PXP", file, "john.doe@evotec.pl", "Evotec", OtpAlgorithm.Sha256, 6, 30);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.OneTimePassword(OtpAuthType.Totp, "JBSWY3DPEHPK3PXP", "john.doe@evotec.pl", "Evotec", OtpAlgorithm.Sha256, 6, 30).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_BezahlCodeAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_bezahl_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateBezahlCodeAsync(QrBezahlAuthorityType.Contact, "Evotec GmbH", "1234567890", "10020030", "DE12500105170648489890", "COBADEFFXXX", "Invoice 2026-041", file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.BezahlCode(QrBezahlAuthorityType.Contact, "Evotec GmbH", "1234567890", "10020030", "DE12500105170648489890", "COBADEFFXXX", "Invoice 2026-041").Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_BezahlCodeSinglePaymentAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_bezahl_single_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateBezahlCodeAsync(QrBezahlAuthorityType.SinglePayment, "Evotec GmbH", "1234567890", "10020030", string.Empty, string.Empty, "Invoice 2026-041", file, 12.34m, executionDate: new System.DateTime(2026, 4, 10));
        Assert.True(File.Exists(file));
        var expected = QrPayloads.BezahlCodeSinglePayment("Evotec GmbH", "1234567890", "10020030", 12.34m, "Invoice 2026-041", "EUR", string.Empty, new System.DateTime(2026, 4, 10)).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_BezahlCodeSingleDirectDebitSepaAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_bezahl_direct_debit_sepa_async.png");
        if (File.Exists(file)) File.Delete(file);
        await QrCode.GenerateBezahlCodeAsync(
            QrBezahlAuthorityType.SingleDirectDebitSepa,
            "Evotec GmbH",
            string.Empty,
            string.Empty,
            "DE12500105170648489890",
            "COBADEFFXXX",
            "Invoice 2026-041",
            file,
            45.67m,
            creditorId: "DE98ZZZ09999999999",
            mandateId: "MANDATE-2026-041",
            dateOfSignature: new System.DateTime(2026, 3, 1),
            executionDate: new System.DateTime(2026, 4, 12));
        Assert.True(File.Exists(file));
        var expected = QrPayloads.BezahlCodeSingleDirectDebitSepa(
            "Evotec GmbH",
            "DE12500105170648489890",
            "COBADEFFXXX",
            45.67m,
            "DE98ZZZ09999999999",
            "MANDATE-2026-041",
            new System.DateTime(2026, 3, 1),
            "Invoice 2026-041",
            "EUR",
            string.Empty,
            new System.DateTime(2026, 4, 12)).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_SwissAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_swiss_async.png");
        if (File.Exists(file)) File.Delete(file);
        var payload = CreateSwissPayload();
        await QrCode.GenerateSwissQrCodeAsync(payload, file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.SwissQrCode(payload).Text;
        AssertQrDecoded(file, expected);
    }

    [Fact]
    public async Task Test_QRCode_SlovenianUpnAsync() {
        string file = Path.Combine(_directoryWithTests, "qr_upn_async.png");
        if (File.Exists(file)) File.Delete(file);
        var payload = CreateSlovenianUpnPayload();
        await QrCode.GenerateSlovenianUpnQrAsync(payload, file);
        Assert.True(File.Exists(file));
        var expected = QrPayloads.SlovenianUpn(payload).Text;
        AssertQrDecoded(file, expected);
    }

    private static SwissQrCodePayload CreateSwissPayload() {
        var iban = new SwissQrCodePayload.Iban("CH4431999123000889012", SwissQrCodePayload.Iban.IbanType.Iban);
        var creditor = SwissQrCodePayload.Contact.CreateStructured("Evotec GmbH", "Main Street", "1", "8000", "Zurich", "CH");
        var reference = new SwissQrCodePayload.Reference(SwissQrCodePayload.Reference.ReferenceType.NON);
        return new SwissQrCodePayload(iban, QrSwissCurrency.CHF, creditor, reference);
    }

    private static SlovenianUpnQrPayload CreateSlovenianUpnPayload() {
        return new SlovenianUpnQrPayload(
            "John Doe",
            "Main Street 1",
            "Ljubljana",
            "Evotec d.o.o.",
            "Business Street 2",
            "Maribor",
            "SI56192001234567890",
            "Invoice 2026-041",
            199.99);
    }
}
