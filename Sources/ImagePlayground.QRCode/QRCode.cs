using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CodeGlyphX;
using CodeGlyphX.Payloads;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using CodeGlyphXPixelFormat = CodeGlyphX.PixelFormat;
using CodeGlyphXRgba32 = CodeGlyphX.Rendering.Png.Rgba32;

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
    public static void Generate(string content, string filePath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(new QrPayloadData(content), filePath, options);
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
    public static void Generate(string content, string filePath, string logoPath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var logoBytes = LoadLogoPng(logoPath);
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize, logoBytes, drawLogoBackground: false);
        RenderToFile(new QrPayloadData(content), filePath, options);
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
        var payload = QrPayloads.Wifi(ssid, password, "WPA", false);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.H, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.WhatsAppMessage(message);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.Url(url);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.Bookmark(bookmarkUrl, bookmarkName);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateCalendarEvent(string calendarEntry, string? calendarMessage, string? calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, QrCalendarEncoding calendarEventEncoding = QrCalendarEncoding.ICalComplete, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    ///   <code>QrCode.GenerateContact("contact.png", QrContactOutputType.MeCard, "John", "Doe");</code>
    /// </example>
    public static void GenerateContact(string filePath, QrContactOutputType outputType, string firstname, string lastname, string? nickname = null, string? phone = null, string? mobilePhone = null, string? workPhone = null, string? email = null, DateTime? birthday = null, string? website = null, string? street = null, string? houseNumber = null, string? city = null, string? zipCode = null, string? country = null, string? note = null, string? stateRegion = null, QrContactAddressOrder addressOrder = QrContactAddressOrder.Default, string? org = null, string? orgTitle = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, QrContactAddressType addressType = QrContactAddressType.HomePreferred) {
        var payload = QrPayloads.Contact(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle, addressType);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateEmail(string filePath, string email, string? subject = null, string? message = null, QrMailEncoding encoding = QrMailEncoding.Mailto, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Email(email, subject, message, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateMMS(string filePath, string phoneNumber, string? subject = null, QrMmsEncoding encoding = QrMmsEncoding.Mmsto, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Mms(phoneNumber, subject, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateSms(string number, string? message, string filePath, QrSmsEncoding encoding = QrSmsEncoding.Sms, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Sms(number, message, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateGeoLocation(string latitude, string longitude, string filePath, QrGeolocationEncoding encoding = QrGeolocationEncoding.Geo, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Geo(latitude, longitude, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateGirocode(string iban, string bic, string name, decimal amount, string filePath, string? remittanceInformation = null, QrGirocodeRemittanceType type = QrGirocodeRemittanceType.Unstructured, string? purposeOfCreditTransfer = null, string? messageToGirocodeUser = null, QrGirocodeVersion version = QrGirocodeVersion.Version1, QrGirocodeEncoding encoding = QrGirocodeEncoding.Iso8859_1, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Girocode(iban, bic, name, amount, remittanceInformation ?? string.Empty, type, purposeOfCreditTransfer ?? string.Empty, messageToGirocodeUser ?? string.Empty, version, encoding);
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    ///   <code>QrCode.GenerateBitcoinAddress(QrBitcoinLikeType.Bitcoin, "addr", 0.5, null, null, "btc.png");</code>
    /// </example>
    public static void GenerateBitcoinAddress(QrBitcoinLikeType currency, string address, double? amount, string? label, string? message, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.BitcoinLike(currency, address, amount, label, message);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.Monero(address, amount, paymentId, recipientName, description);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.Phone(number);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
        var payload = QrPayloads.SkypeCall(username);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    ///   <code>QrCode.GenerateShadowSocks("host", 8388, "pass", QrShadowSocksMethod.Aes256Gcm, "ss.png");</code>
    /// </example>
    public static void GenerateShadowSocks(string hostname, int port, string password, QrShadowSocksMethod method, string filePath, string? tag = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.ShadowSocks(hostname, port, password, method, tag);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
    }

    /// <summary>
    /// Generates a QR code for configuring a one-time password generator.
    /// </summary>
    /// <param name="type">OTP type.</param>
    /// <param name="secretBase32">Base32-encoded secret.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="label">Optional label.</param>
    /// <param name="issuer">Optional issuer.</param>
    /// <param name="algorithm">Hash algorithm.</param>
    /// <param name="digits">Number of digits.</param>
    /// <param name="period">Period for TOTP.</param>
    /// <param name="counter">Counter for HOTP.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateOneTimePassword(OtpAuthType.Totp, "BASE32SECRET", "otp.png");</code>
    /// </example>
    public static void GenerateOneTimePassword(OtpAuthType type, string secretBase32, string filePath, string? label = null, string? issuer = null, OtpAlgorithm algorithm = OtpAlgorithm.Sha1, int digits = 6, int? period = 30, int? counter = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.OneTimePassword(type, secretBase32, label, issuer, algorithm, digits, period, counter);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateSlovenianUpnQr(SlovenianUpnQrPayload upn, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        if (upn is null) {
            throw new ArgumentNullException(nameof(upn));
        }
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(upn.ToPayloadData(), filePath, options);
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
    ///   <code>QrCode.GenerateBezahlCode(QrBezahlAuthorityType.Contact, "John", "123", "10020030", "DE89...", "BICCODE", "Invoice", "bezahl.png");</code>
    /// </example>
    public static void GenerateBezahlCode(QrBezahlAuthorityType authority, string name, string account, string bnc, string iban, string bic, string reason, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.BezahlCode(authority, name, account, bnc, iban, bic, reason);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
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
    public static void GenerateSwissQrCode(SwissQrCodePayload swiss, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        if (swiss is null) {
            throw new ArgumentNullException(nameof(swiss));
        }
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(swiss.ToPayloadData(), filePath, options);
    }

    private static QrEasyOptions BuildOptions(bool transparent, QrErrorCorrectionLevel? eccLevel, Color? foregroundColor, Color? backgroundColor, int pixelSize, byte[]? logoPng = null, bool drawLogoBackground = true) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }

        Color fg = foregroundColor ?? Color.Black;
        Color bg = transparent ? Color.Transparent : (backgroundColor ?? Color.White);
        var options = new QrEasyOptions {
            ModuleSize = pixelSize,
            Foreground = ToCodeGlyphXColor(fg),
            Background = ToCodeGlyphXColor(bg)
        };
        if (eccLevel.HasValue) {
            options.ErrorCorrectionLevel = eccLevel;
        }

        if (logoPng is { Length: > 0 }) {
            options.LogoPng = logoPng;
            options.LogoDrawBackground = drawLogoBackground;
        }

        return options;
    }

    private static CodeGlyphXRgba32 ToCodeGlyphXColor(Color color) {
        var px = color.ToPixel<Rgba32>();
        return new CodeGlyphXRgba32(px.R, px.G, px.B, px.A);
    }

    private static void RenderToFile(QrPayloadData payload, string filePath, QrEasyOptions options) {
        string fullPath = Helpers.ResolvePath(filePath);
        string extension = Path.GetExtension(fullPath);
        if (string.IsNullOrWhiteSpace(extension)) {
            throw new UnknownImageFormatException(
                $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}");
        }

        CodeGlyphX.QrCode.Save(payload, fullPath, options);
    }

    private static byte[] LoadLogoPng(string logoPath) {
        string fullLogoPath = Helpers.ResolvePath(logoPath);
        using Image<Rgba32> logo = SixLabors.ImageSharp.Image.Load<Rgba32>(fullLogoPath);
        using MemoryStream ms = new();
        logo.SaveAsPng(ms);
        return ms.ToArray();
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
        #if NET8_0_OR_GREATER
        using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(fullPath);
        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);

        if (TryDecodePixels(pixels, image.Width, image.Height, out var decoded)) {
            return new BarcodeResult<Rgba32> {
                Status = Status.Found,
                Message = decoded.Text,
                Value = decoded.Text
            };
        }

        var composited = CompositeOnWhite(image);
        if (composited is not null && TryDecodePixels(composited, image.Width, image.Height, out decoded)) {
            return new BarcodeResult<Rgba32> {
                Status = Status.Found,
                Message = decoded.Text,
                Value = decoded.Text
            };
        }

        if (TryDecodeImageFallback(fullPath, out decoded)) {
            return new BarcodeResult<Rgba32> {
                Status = Status.Found,
                Message = decoded.Text,
                Value = decoded.Text
            };
        }

        return new BarcodeResult<Rgba32> {
            Status = Status.NotFound
        };
        #else
        return new BarcodeResult<Rgba32> {
            Status = Status.Error,
            Message = "QR decoding requires .NET 8.0 or newer."
        };
        #endif

    }

    #if NET8_0_OR_GREATER
    private static bool TryDecodeImageFallback(string fullPath, out QrDecoded decoded) {
        var aggressiveOptions = new QrPixelDecodeOptions {
            Profile = QrDecodeProfile.Robust,
            AggressiveSampling = true,
            StylizedSampling = true
        };

        if (QrImageDecoder.TryDecodeImage(File.ReadAllBytes(fullPath), aggressiveOptions, out decoded)) {
            return true;
        }

        using FileStream stream = File.OpenRead(fullPath);
        return QrImageDecoder.TryDecodeImage(stream, aggressiveOptions, out decoded);
    }

    private static bool TryDecodePixels(byte[] pixels, int width, int height, out QrDecoded decoded) {
        if (QrImageDecoder.TryDecode(pixels, width, height, width * 4, CodeGlyphXPixelFormat.Rgba32, out decoded)) {
            return true;
        }

        var aggressiveOptions = new QrPixelDecodeOptions {
            Profile = QrDecodeProfile.Robust,
            AggressiveSampling = true,
            StylizedSampling = true
        };
        return QrImageDecoder.TryDecode(pixels, width, height, width * 4, CodeGlyphXPixelFormat.Rgba32, aggressiveOptions, out decoded);
    }
    #endif

    private static byte[]? CompositeOnWhite(Image<Rgba32> image) {
        byte[]? composited = null;
        var index = 0;

        for (var y = 0; y < image.Height; y++) {
            for (var x = 0; x < image.Width; x++) {
                Rgba32 pixel = image[x, y];
                if (pixel.A == 255) {
                    if (composited is not null) {
                        composited[index] = pixel.R;
                        composited[index + 1] = pixel.G;
                        composited[index + 2] = pixel.B;
                        composited[index + 3] = 255;
                    }

                    index += 4;
                    continue;
                }

                composited ??= new byte[image.Width * image.Height * 4];
                if (index > 0) {
                    image.CopyPixelDataTo(composited);
                }

                byte alpha = pixel.A;
                composited[index] = BlendChannel(pixel.R, alpha);
                composited[index + 1] = BlendChannel(pixel.G, alpha);
                composited[index + 2] = BlendChannel(pixel.B, alpha);
                composited[index + 3] = 255;
                index += 4;
            }
        }

        return composited;
    }

    private static byte BlendChannel(byte foreground, byte alpha) {
        const int background = 255;
        return (byte)((foreground * alpha + background * (255 - alpha) + 127) / 255);
    }
}




