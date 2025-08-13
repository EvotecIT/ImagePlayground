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
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void Generate(string content, string filePath, bool transparent = false, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }

        string fullPath = Helpers.ResolvePath(filePath);

        FileInfo fileInfo = new FileInfo(fullPath);

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator()) {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel)) {
                using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) {
                    Color dark = foregroundColor ?? Color.Black;
                    Color light = transparent ? Color.Transparent : (backgroundColor ?? Color.White);
                    using (var qrCodeImage = qrCode.GetGraphic(pixelSize, dark, light, true)) {
                        Helpers.CreateParentDirectory(fullPath);
                        switch (fileInfo.Extension.ToLowerInvariant()) {
                            case ".png":
                                qrCodeImage.SaveAsPng(fullPath);
                                break;
                            case ".jpg":
                            case ".jpeg":
                                qrCodeImage.SaveAsJpeg(fullPath);
                                break;
                            case ".ico":
                                SaveImageAsIcon(qrCodeImage, fullPath);
                                break;
                            default:
                                throw new UnknownImageFormatException(
                                    $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}");
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Creates a QR code image from a raw string and overlays a logo at the center.
    /// </summary>
    /// <param name="content">Content to encode.</param>
    /// <param name="filePath">Path where the QR image will be saved.</param>
    /// <param name="logoPath">Path to the logo image to overlay.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="eccLevel">Error correction level.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void Generate(string content, string filePath, string logoPath, bool transparent = false, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }

        string fullPath = Helpers.ResolvePath(filePath);
        string fullLogoPath = Helpers.ResolvePath(logoPath);

        FileInfo fileInfo = new FileInfo(fullPath);

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator()) {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel)) {
                using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) {
                    Color dark = foregroundColor ?? Color.Black;
                    Color light = transparent ? Color.Transparent : (backgroundColor ?? Color.White);
                    using (var qrCodeImage = qrCode.GetGraphic(pixelSize, dark, light, true)) {
                        using (SixLabors.ImageSharp.Image<Rgba32> logo = SixLabors.ImageSharp.Image.Load<Rgba32>(fullLogoPath)) {
                            int logoSize = qrCodeImage.Width / 5;
                            logo.Mutate(x => x.Resize(new ResizeOptions {
                                Mode = ResizeMode.Max,
                                Size = new Size(logoSize, logoSize)
                            }));
                            int posX = (qrCodeImage.Width - logo.Width) / 2;
                            int posY = (qrCodeImage.Height - logo.Height) / 2;
                            qrCodeImage.Mutate(ctx => ctx.DrawImage(logo, new Point(posX, posY), 1f));
                        }

                        Helpers.CreateParentDirectory(fullPath);
                        string extension = fileInfo.Extension.ToLowerInvariant();
                        Action saveAction = extension switch {
                            ".png" => () => qrCodeImage.SaveAsPng(fullPath),
                            ".jpg" or ".jpeg" => () => qrCodeImage.SaveAsJpeg(fullPath),
                            ".ico" => () => SaveImageAsIcon(qrCodeImage, fullPath),
                            _ => throw new UnknownImageFormatException(
                                $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}")
                        };
                        saveAction();
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
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateWiFi(string ssid, string password, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        string encodedPassword = WebUtility.UrlEncode(password);
        PayloadGenerator.WiFi generator = new PayloadGenerator.WiFi(ssid, encodedPassword, PayloadGenerator.WiFi.Authentication.WPA, false, false);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Creates a QR code that opens WhatsApp with a pre-filled message.
    /// </summary>
    /// <param name="message">Message text to pre-fill.</param>
    /// <param name="filePath">Destination path for the QR image.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateWhatsAppMessage(string message, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        PayloadGenerator.WhatsAppMessage generator = new PayloadGenerator.WhatsAppMessage(message);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }
    /// <summary>
    /// Generates a QR code representing a hyperlink.
    /// </summary>
    /// <param name="url">URL to encode.</param>
    /// <param name="filePath">Destination path for the QR image.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateUrl(string url, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        PayloadGenerator.Url generator = new PayloadGenerator.Url(url);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }
    /// <summary>
    /// Generates a QR code that opens a bookmark in the browser.
    /// </summary>
    /// <param name="bookmarkUrl">URL of the bookmark.</param>
    /// <param name="bookmarkName">Bookmark display name.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateBookmark(string bookmarkUrl, string bookmarkName, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        PayloadGenerator.Bookmark generator = new PayloadGenerator.Bookmark(bookmarkUrl, bookmarkName);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
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
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateCalendarEvent(string calendarEntry, string calendarMessage, string calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, PayloadGenerator.CalendarEvent.EventEncoding calendarEventEncoding = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        PayloadGenerator.CalendarEvent generator = new PayloadGenerator.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code containing contact information in vCard format.
    /// </summary>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="outputType">Format of the contact payload.</param>
    /// <param name="firstname">Contact first name.</param>
    /// <param name="lastname">Contact last name.</param>
    /// <param name="nickname">Optional nickname.</param>
    /// <param name="phone">Primary phone number.</param>
    /// <param name="mobilePhone">Mobile phone number.</param>
    /// <param name="workPhone">Work phone number.</param>
    /// <param name="email">Email address.</param>
    /// <param name="birthday">Birth date of the contact.</param>
    /// <param name="website">Website URL.</param>
    /// <param name="street">Street name.</param>
    /// <param name="houseNumber">House number.</param>
    /// <param name="city">City or town.</param>
    /// <param name="zipCode">Postal code.</param>
    /// <param name="country">Country name.</param>
    /// <param name="note">Additional notes.</param>
    /// <param name="stateRegion">State or region.</param>
    /// <param name="addressOrder">Ordering of address components.</param>
    /// <param name="org">Organization name.</param>
    /// <param name="orgTitle">Organization title.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateContact("contact.png", PayloadGenerator.ContactData.ContactOutputType.MeCard, "John", "Doe");</code>
    /// </example>
    public static void GenerateContact(string filePath, PayloadGenerator.ContactData.ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, PayloadGenerator.ContactData.AddressOrder addressOrder = PayloadGenerator.ContactData.AddressOrder.Default, string org = null, string orgTitle = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        PayloadGenerator.ContactData generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code that opens an email draft.
    /// </summary>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="email">Recipient email address.</param>
    /// <param name="subject">Optional message subject.</param>
    /// <param name="message">Optional message body.</param>
    /// <param name="encoding">Mail encoding scheme.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateEmail("email.png", "user@example.com", "Hello", "Body");</code>
    /// </example>
    public static void GenerateEmail(string filePath, string email, string subject = null, string message = null, PayloadGenerator.Mail.MailEncoding encoding = PayloadGenerator.Mail.MailEncoding.MAILTO, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }

        PayloadGenerator.Mail generator = new PayloadGenerator.Mail(email, subject, message, encoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code that opens the messaging app with an MMS draft.
    /// </summary>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="phoneNumber">Recipient phone number.</param>
    /// <param name="subject">Subject line for the MMS message.</param>
    /// <param name="encoding">MMS encoding scheme.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateMMS("mms.png", "+123456789", "Hi");</code>
    /// </example>
    public static void GenerateMMS(string filePath, string phoneNumber, string subject = "", PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMSTO, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = subject == "" ? new PayloadGenerator.MMS(phoneNumber, encoding) : new PayloadGenerator.MMS(phoneNumber, subject, encoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Creates a QR code that launches the SMS application.
    /// </summary>
    /// <param name="number">Phone number to message.</param>
    /// <param name="message">Optional message body.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="encoding">SMS encoding type.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateSms("+123456789", "Hello", "sms.png");</code>
    /// </example>
    public static void GenerateSms(string number, string? message, string filePath, PayloadGenerator.SMS.SMSEncoding encoding = PayloadGenerator.SMS.SMSEncoding.SMS, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = string.IsNullOrEmpty(message)
            ? new PayloadGenerator.SMS(number, encoding)
            : new PayloadGenerator.SMS(number, message!, encoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code representing a geographic location.
    /// </summary>
    /// <param name="latitude">Latitude coordinate.</param>
    /// <param name="longitude">Longitude coordinate.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="encoding">Geolocation encoding scheme.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateGeoLocation("46.0569", "14.5058", "geo.png");</code>
    /// </example>
    public static void GenerateGeoLocation(string latitude, string longitude, string filePath, PayloadGenerator.Geolocation.GeolocationEncoding encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Creates a Girocode compliant QR code for SEPA credit transfers.
    /// </summary>
    /// <param name="iban">IBAN of the receiver.</param>
    /// <param name="bic">BIC of the receiver.</param>
    /// <param name="name">Name of the receiver.</param>
    /// <param name="amount">Transfer amount.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="remittanceInformation">Remittance information.</param>
    /// <param name="type">Type of remittance information.</param>
    /// <param name="purposeOfCreditTransfer">Purpose of the credit transfer.</param>
    /// <param name="messageToGirocodeUser">Message displayed to the user.</param>
    /// <param name="version">Girocode version.</param>
    /// <param name="encoding">Character encoding.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateGirocode("DE02120300000000202051", "BYLADEM1001", "ACME", 10m, "giro.png");</code>
    /// </example>
    public static void GenerateGirocode(string iban, string bic, string name, decimal amount, string filePath, string? remittanceInformation = null, PayloadGenerator.Girocode.TypeOfRemittance type = PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, string? purposeOfCreditTransfer = null, string? messageToGirocodeUser = null, PayloadGenerator.Girocode.GirocodeVersion version = PayloadGenerator.Girocode.GirocodeVersion.Version1, PayloadGenerator.Girocode.GirocodeEncoding encoding = PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation ?? string.Empty, type, purposeOfCreditTransfer ?? string.Empty, messageToGirocodeUser ?? string.Empty, version, encoding);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code for Bitcoin-like cryptocurrency payments.
    /// </summary>
    /// <param name="currency">Cryptocurrency type.</param>
    /// <param name="address">Destination address.</param>
    /// <param name="amount">Optional transfer amount.</param>
    /// <param name="label">Optional label.</param>
    /// <param name="message">Optional message.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateBitcoinAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.Bitcoin, "addr", 0.5, null, null, "btc.png");</code>
    /// </example>
    public static void GenerateBitcoinAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType currency, string address, double? amount, string? label, string? message, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(currency, address, amount, label, message);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code for a Monero transfer.
    /// </summary>
    /// <param name="address">Destination address.</param>
    /// <param name="amount">Optional transfer amount.</param>
    /// <param name="paymentId">Optional payment identifier.</param>
    /// <param name="recipientName">Name of the recipient.</param>
    /// <param name="description">Transaction description.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateMoneroTransaction("addr", 1.0f, null, null, null, "xmr.png");</code>
    /// </example>
    public static void GenerateMoneroTransaction(string address, float? amount, string? paymentId, string? recipientName, string? description, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.MoneroTransaction(address, amount, paymentId, recipientName, description);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code that initiates a phone call.
    /// </summary>
    /// <param name="number">Phone number to call.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GeneratePhoneNumber("+123456789", "phone.png");</code>
    /// </example>
    public static void GeneratePhoneNumber(string number, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.PhoneNumber(number);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code that starts a Skype call.
    /// </summary>
    /// <param name="username">Skype username.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateSkypeCall("user", "skype.png");</code>
    /// </example>
    public static void GenerateSkypeCall(string username, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.SkypeCall(username);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code containing Shadowsocks configuration settings.
    /// </summary>
    /// <param name="hostname">Server hostname.</param>
    /// <param name="port">Server port.</param>
    /// <param name="password">Connection password.</param>
    /// <param name="method">Encryption method.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="tag">Optional connection tag.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateShadowSocks("host", 8388, "pass", PayloadGenerator.ShadowSocksConfig.Method.AES_256_GCM, "ss.png");</code>
    /// </example>
    public static void GenerateShadowSocks(string hostname, int port, string password, PayloadGenerator.ShadowSocksConfig.Method method, string filePath, string? tag = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.ShadowSocksConfig(hostname, port, password, method, tag);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a QR code for configuring a one-time password generator.
    /// </summary>
    /// <param name="otp">Payload with OTP configuration.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateOneTimePassword(otp, "otp.png");</code>
    /// </example>
    public static void GenerateOneTimePassword(PayloadGenerator.OneTimePassword otp, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        Generate(otp.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }


    /// <summary>
    /// Generates a Slovenian UPN QR payment code.
    /// </summary>
    /// <param name="upn">Payload with Slovenian UPN payment details.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateSlovenianUpnQr(upn, "upn.png");</code>
    /// </example>
    public static void GenerateSlovenianUpnQr(PayloadGenerator.SlovenianUpnQr upn, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        Generate(upn.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a German BezahlCode payment QR code.
    /// </summary>
    /// <param name="authority">Authority type.</param>
    /// <param name="name">Creditor name.</param>
    /// <param name="account">Account number.</param>
    /// <param name="bnc">Bank number.</param>
    /// <param name="iban">IBAN of the creditor.</param>
    /// <param name="bic">BIC of the creditor.</param>
    /// <param name="reason">Payment reason.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateBezahlCode(PayloadGenerator.BezahlCode.AuthorityType.Person, "John", "123", "10020030", "DE89...", "BICCODE", "Invoice", "bezahl.png");</code>
    /// </example>
    public static void GenerateBezahlCode(PayloadGenerator.BezahlCode.AuthorityType authority, string name, string account, string bnc, string iban, string bic, string reason, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var generator = new PayloadGenerator.BezahlCode(authority, name, account, bnc, iban, bic, reason);
        Generate(generator.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Generates a Swiss QR invoice code.
    /// </summary>
    /// <param name="swiss">Swiss QR code payload.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateSwissQrCode(swissPayload, "swiss.png");</code>
    /// </example>
    public static void GenerateSwissQrCode(PayloadGenerator.SwissQrCode swiss, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        Generate(swiss.ToString(), filePath, transparent, QRCodeGenerator.ECCLevel.Q, foregroundColor, backgroundColor, pixelSize);
    }

    /// <summary>
    /// Reads a QR code image and returns the decoded payload.
    /// </summary>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <returns>Decoded barcode result.</returns>
    /// <example>
    ///   <code>var result = QrCode.Read("code.png");</code>
    /// </example>
    public static BarcodeResult<Rgba32> Read(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        using Image<Rgba32> barcodeImage = SixLabors.ImageSharp.Image.Load<Rgba32>(fullPath);
        BarcodeReader<Rgba32> reader = new BarcodeReader<Rgba32>(types: ZXing.BarcodeFormat.QR_CODE);
        BarcodeResult<Rgba32> response = reader.Decode(barcodeImage);
        response.Image?.Dispose();
        BarcodeResult<Rgba32> result = new() {
            Value = response.Value,
            Status = response.Status,
            Message = response.Message
        };
        return result;
    }

    /// <summary>
    /// Saves an ImageSharp image as an ICO file with multiple resolutions.
    /// </summary>
    /// <param name="image">Image to save.</param>
    /// <param name="filePath">Destination ICO file path.</param>
    /// <param name="sizes">Icon sizes to include.</param>
    private static void SaveImageAsIcon(SixLabors.ImageSharp.Image image, string filePath, params int[] sizes) {
        Helpers.CreateParentDirectory(filePath);
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
