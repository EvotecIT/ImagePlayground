using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BarcodeReader.ImageSharp;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for generating various QR code payloads and reading QR codes.
/// </summary>
public class QrCode {
    /// <summary>
    /// Creates a QR code image from a raw string value.
    /// </summary>
    /// <param name="content">Content to encode.</param>
    /// <param name="filePath">Path where the QR image will be saved.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="eccLevel">Error correction level.</param>
    public static void Generate(string content, string filePath, bool transparent = false, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q) {
        string fullPath = Helpers.ResolvePath(filePath);

        FileInfo fileInfo = new FileInfo(fullPath);

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator()) {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel)) {
                using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) {
                    using (var qrCodeImage = qrCode.GetGraphic(20)) {
                        if (transparent) {
                            //qrCodeImage.MakeTransparent();
                        }
                        // this uses QRCoder
                        //ImageFormat imageFormatDetected;
                        //if (fileInfo.Extension == ".png") {
                        //    imageFormatDetected = ImageFormat.Png;
                        //} else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                        //    imageFormatDetected = ImageFormat.Jpeg;
                        //} else if (fileInfo.Extension == ".ico") {
                        //    imageFormatDetected = ImageFormat.Icon;
                        //} else {
                        //    throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
                        //}
                        //qrCodeImage.Save(filePath, imageFormatDetected);

                        //this uses QRCoder.ImageSharp
                        if (fileInfo.Extension == ".png") {
                            qrCodeImage.SaveAsPng(fullPath);
                        } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                            qrCodeImage.SaveAsJpeg(fullPath);
                        } else if (fileInfo.Extension == ".ico") {
                            SaveImageAsIcon(qrCodeImage, fullPath);
                        } else {
                            throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Creates a QR code containing WiFi configuration information.
    /// </summary>
    /// <param name="ssid">Wireless network SSID.</param>
    /// <param name="password">Wireless password.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    public static void GenerateWiFi(string ssid, string password, string filePath, bool transparent = false) {
        string encodedPassword = WebUtility.UrlEncode(password);
        PayloadGenerator.WiFi generator = new PayloadGenerator.WiFi(ssid, encodedPassword, PayloadGenerator.WiFi.Authentication.WPA, false, false);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Creates a QR code that opens WhatsApp with a pre-filled message.
    /// </summary>
    /// <param name="message">Message text to pre-fill.</param>
    /// <param name="filePath">Destination path for the QR image.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    public static void GenerateWhatsAppMessage(string message, string filePath, bool transparent = false) {
        PayloadGenerator.WhatsAppMessage generator = new PayloadGenerator.WhatsAppMessage(message);
        Generate(generator.ToString(), filePath, transparent);
    }
    /// <summary>
    /// Generates a QR code representing a hyperlink.
    /// </summary>
    /// <param name="url">URL to encode.</param>
    /// <param name="filePath">Destination path for the QR image.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    public static void GenerateUrl(string url, string filePath, bool transparent = false) {
        PayloadGenerator.Url generator = new PayloadGenerator.Url(url);
        Generate(generator.ToString(), filePath, transparent);
    }
    /// <summary>
    /// Generates a QR code that opens a bookmark in the browser.
    /// </summary>
    /// <param name="bookmarkUrl">URL of the bookmark.</param>
    /// <param name="bookmarkName">Bookmark display name.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    public static void GenerateBookmark(string bookmarkUrl, string bookmarkName, string filePath, bool transparent = false) {
        PayloadGenerator.Bookmark generator = new PayloadGenerator.Bookmark(bookmarkUrl, bookmarkName);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Creates a QR code for a calendar event using the iCalendar specification.
    /// </summary>
    /// <param name="calendarEntry">Event title.</param>
    /// <param name="calendarMessage">Event description.</param>
    /// <param name="calendarGeoLocation">Event location.</param>
    /// <param name="calendarFrom">Start date.</param>
    /// <param name="calendarTo">End date.</param>
    /// <param name="filePath">Output image path.</param>
    /// <param name="allDayEvent">Specifies whether the event spans the full day.</param>
    /// <param name="calendarEventEncoding">Calendar encoding.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    public static void GenerateCalendarEvent(string calendarEntry, string calendarMessage, string calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, PayloadGenerator.CalendarEvent.EventEncoding calendarEventEncoding = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete, bool transparent = false) {
        PayloadGenerator.CalendarEvent generator = new PayloadGenerator.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code containing contact information in vCard format.
    /// </summary>
    public static void GenerateContact(string filePath, PayloadGenerator.ContactData.ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, PayloadGenerator.ContactData.AddressOrder addressOrder = PayloadGenerator.ContactData.AddressOrder.Default, string org = null, string orgTitle = null, bool transparent = false) {
        PayloadGenerator.ContactData generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code that opens an email draft.
    /// </summary>
    public static void GenerateEmail(string filePath, string email, string subject = null, string message = null, PayloadGenerator.Mail.MailEncoding encoding = PayloadGenerator.Mail.MailEncoding.MAILTO, bool transparent = false) {
        PayloadGenerator.Mail generator = new PayloadGenerator.Mail(email, subject, message, encoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code that opens the messaging app with an MMS draft.
    /// </summary>
    public static void GenerateMMS(string filePath, string phoneNumber, string subject = "", PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMSTO, bool transparent = false) {
        var generator = subject == "" ? new PayloadGenerator.MMS(phoneNumber, encoding) : new PayloadGenerator.MMS(phoneNumber, subject, encoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Creates a QR code that launches the SMS application.
    /// </summary>
    public static void GenerateSms(string number, string? message, string filePath, PayloadGenerator.SMS.SMSEncoding encoding = PayloadGenerator.SMS.SMSEncoding.SMS, bool transparent = false) {
        var generator = string.IsNullOrEmpty(message)
            ? new PayloadGenerator.SMS(number, encoding)
            : new PayloadGenerator.SMS(number, message!, encoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code representing a geographic location.
    /// </summary>
    public static void GenerateGeoLocation(string latitude, string longitude, string filePath, PayloadGenerator.Geolocation.GeolocationEncoding encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO, bool transparent = false) {
        var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Creates a Girocode compliant QR code for SEPA credit transfers.
    /// </summary>
    public static void GenerateGirocode(string iban, string bic, string name, decimal amount, string filePath, string? remittanceInformation = null, PayloadGenerator.Girocode.TypeOfRemittance type = PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, string? purposeOfCreditTransfer = null, string? messageToGirocodeUser = null, PayloadGenerator.Girocode.GirocodeVersion version = PayloadGenerator.Girocode.GirocodeVersion.Version1, PayloadGenerator.Girocode.GirocodeEncoding encoding = PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1, bool transparent = false) {
        var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation ?? string.Empty, type, purposeOfCreditTransfer ?? string.Empty, messageToGirocodeUser ?? string.Empty, version, encoding);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code for Bitcoin-like cryptocurrency payments.
    /// </summary>
    public static void GenerateBitcoinAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType currency, string address, double? amount, string? label, string? message, string filePath, bool transparent = false) {
        var generator = new PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(currency, address, amount, label, message);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code for a Monero transfer.
    /// </summary>
    public static void GenerateMoneroTransaction(string address, float? amount, string? paymentId, string? recipientName, string? description, string filePath, bool transparent = false) {
        var generator = new PayloadGenerator.MoneroTransaction(address, amount, paymentId, recipientName, description);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code that initiates a phone call.
    /// </summary>
    public static void GeneratePhoneNumber(string number, string filePath, bool transparent = false) {
        var generator = new PayloadGenerator.PhoneNumber(number);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code that starts a Skype call.
    /// </summary>
    public static void GenerateSkypeCall(string username, string filePath, bool transparent = false) {
        var generator = new PayloadGenerator.SkypeCall(username);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code containing Shadowsocks configuration settings.
    /// </summary>
    public static void GenerateShadowSocks(string hostname, int port, string password, PayloadGenerator.ShadowSocksConfig.Method method, string filePath, string? tag = null, bool transparent = false) {
        var generator = new PayloadGenerator.ShadowSocksConfig(hostname, port, password, method, tag);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a QR code for configuring a one-time password generator.
    /// </summary>
    public static void GenerateOneTimePassword(PayloadGenerator.OneTimePassword otp, string filePath, bool transparent = false) {
        Generate(otp.ToString(), filePath, transparent);
    }


    /// <summary>
    /// Generates a Slovenian UPN QR payment code.
    /// </summary>
    public static void GenerateSlovenianUpnQr(PayloadGenerator.SlovenianUpnQr upn, string filePath, bool transparent = false) {
        Generate(upn.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a German BezahlCode payment QR code.
    /// </summary>
    public static void GenerateBezahlCode(PayloadGenerator.BezahlCode.AuthorityType authority, string name, string account, string bnc, string iban, string bic, string reason, string filePath, bool transparent = false) {
        var generator = new PayloadGenerator.BezahlCode(authority, name, account, bnc, iban, bic, reason);
        Generate(generator.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Generates a Swiss QR invoice code.
    /// </summary>
    public static void GenerateSwissQrCode(PayloadGenerator.SwissQrCode swiss, string filePath, bool transparent = false) {
        Generate(swiss.ToString(), filePath, transparent);
    }

    /// <summary>
    /// Reads a QR code image and returns the decoded payload.
    /// </summary>
    public static BarcodeResult<Rgba32> Read(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        Image<Rgba32> barcodeImage = SixLabors.ImageSharp.Image.Load<Rgba32>(fullPath);
        BarcodeReader<Rgba32> reader = new BarcodeReader<Rgba32>(types: ZXing.BarcodeFormat.QR_CODE);
        var response = reader.Decode(barcodeImage);
        return response;
    }

    /// <summary>
    /// Saves an ImageSharp image as an ICO file with multiple resolutions.
    /// </summary>
    private static void SaveImageAsIcon(SixLabors.ImageSharp.Image image, string filePath, params int[] sizes) {
        if (sizes == null || sizes.Length == 0) {
            sizes = new[] { 16, 32, 48, 64, 128, 256 };
        }

        List<byte[]> frames = new();
        List<(int Width, int Height)> dims = new();

        foreach (int size in sizes.Distinct().OrderBy(s => s)) {
            using Image<Rgba32> clone = image.CloneAs<Rgba32>();
            clone.Mutate(ctx => ctx.Resize(new ResizeOptions {
                Mode = ResizeMode.Stretch,
                Size = new Size(size, size)
            }));
            using MemoryStream ms = new();
            clone.SaveAsPng(ms);
            frames.Add(ms.ToArray());
            dims.Add((size, size));
        }

        using FileStream fs = File.Create(filePath);
        using BinaryWriter bw = new(fs);
        bw.Write((ushort)0);
        bw.Write((ushort)1);
        bw.Write((ushort)frames.Count);

        int offset = 6 + 16 * frames.Count;
        for (int i = 0; i < frames.Count; i++) {
            var (w, h) = dims[i];
            bw.Write((byte)(w >= 256 ? 0 : w));
            bw.Write((byte)(h >= 256 ? 0 : h));
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write((ushort)1);
            bw.Write((ushort)32);
            bw.Write(frames[i].Length);
            bw.Write(offset);
            offset += frames[i].Length;
        }

        foreach (byte[] data in frames) {
            bw.Write(data);
        }

        bw.Flush();
    }
}
