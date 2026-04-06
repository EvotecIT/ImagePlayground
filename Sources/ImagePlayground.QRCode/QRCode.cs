using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Payloads;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using CodeGlyphXPixelFormat = CodeGlyphX.PixelFormat;
using CodeGlyphXRgba32 = CodeGlyphX.Rendering.Png.Rgba32;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for generating QR codes for common payload types and for decoding QR images.
/// <para>
/// The helpers in this class wrap CodeGlyphX payload builders so callers can create QR codes for plain text,
/// URLs, contacts, payment payloads, OTP secrets, WiFi settings, and similar scenarios without manually
/// constructing payload strings.
/// </para>
/// <para>
/// Output format is inferred from the destination file extension, and decode methods return a rich
/// <see cref="BarcodeResult{TPixel}"/> value rather than throwing when no QR code is found.
/// </para>
/// </summary>
public class QrCode {
    /// <summary>
    /// Creates a QR code image from a raw string value.
    /// </summary>
    /// <para>Use this overload when the payload is already prepared as plain text.</para>
    /// <param name="content">Raw content to encode into the QR payload.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="transparent">When set, the QR background is rendered transparent instead of using a solid fill.</param>
    /// <param name="eccLevel">Error correction level used when generating QR modules.</param>
    /// <param name="foregroundColor">Foreground color used for dark QR modules. Defaults to black.</param>
    /// <param name="backgroundColor">Background color used for non-transparent output. Defaults to white.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.Generate("https://evotec.xyz", "qr.png");</code>
    /// </example>
    public static void Generate(string content, string filePath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(new QrPayloadData(content), filePath, options);
    }

    /// <summary>
    /// Creates a QR code image from a raw string value asynchronously.
    /// </summary>
    /// <param name="content">Raw content to encode into the QR payload.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="transparent">When set, the QR background is rendered transparent instead of using a solid fill.</param>
    /// <param name="eccLevel">Error correction level used when generating QR modules.</param>
    /// <param name="foregroundColor">Foreground color used for dark QR modules. Defaults to black.</param>
    /// <param name="backgroundColor">Background color used for non-transparent output. Defaults to white.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateAsync(string content, string filePath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(new QrPayloadData(content), filePath, options, cancellationToken);
    }
    /// <summary>
    /// Creates a QR code image from a raw string and overlays a logo at the center.
    /// </summary>
    /// <para>Use this overload when the QR code should include a small centered logo for branding.</para>
    /// <param name="content">Raw content to encode into the QR payload.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="logoPath">Path to the logo image that should be converted to PNG and placed in the center.</param>
    /// <param name="transparent">When set, the QR background is rendered transparent instead of using a solid fill.</param>
    /// <param name="eccLevel">Error correction level used when generating QR modules.</param>
    /// <param name="foregroundColor">Foreground color used for dark QR modules. Defaults to black.</param>
    /// <param name="backgroundColor">Background color used for non-transparent output. Defaults to white.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.Generate("https://evotec.xyz", "qr-logo.png", "logo.png");</code>
    /// </example>
    public static void Generate(string content, string filePath, string logoPath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize);
        RenderToFileWithCenteredLogo(new QrPayloadData(content), filePath, logoPath, options);
    }

    /// <summary>
    /// Creates a QR code image from a raw string and overlays a logo at the center asynchronously.
    /// </summary>
    /// <param name="content">Raw content to encode into the QR payload.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="logoPath">Path to the logo image that should be converted to PNG and placed in the center.</param>
    /// <param name="transparent">When set, the QR background is rendered transparent instead of using a solid fill.</param>
    /// <param name="eccLevel">Error correction level used when generating QR modules.</param>
    /// <param name="foregroundColor">Foreground color used for dark QR modules. Defaults to black.</param>
    /// <param name="backgroundColor">Background color used for non-transparent output. Defaults to white.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateAsync(string content, string filePath, string logoPath, bool transparent = false, QrErrorCorrectionLevel eccLevel = QrErrorCorrectionLevel.Q, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var options = BuildOptions(transparent, eccLevel, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileWithCenteredLogoAsync(new QrPayloadData(content), filePath, logoPath, options, cancellationToken);
    }
    /// <summary>
    /// Creates a QR code containing WiFi configuration information.
    /// </summary>
    /// <para>The generated QR code uses WPA payload semantics and a higher default correction level for scan reliability.</para>
    /// <param name="ssid">Wireless network SSID.</param>
    /// <param name="password">Wireless network password.</param>
    /// <param name="filePath">Destination image path. The image format is inferred from the extension.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <example>
    ///   <code>QrCode.GenerateWiFi("OfficeWiFi", "pass123!", "wifi.png");</code>
    /// </example>
    public static void GenerateWiFi(string ssid, string password, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.Wifi(ssid, password, "WPA", false);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.H, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
    }

    /// <summary>
    /// Creates a QR code containing WiFi configuration information asynchronously.
    /// </summary>
    /// <param name="ssid">Wireless network SSID.</param>
    /// <param name="password">Wireless network password.</param>
    /// <param name="filePath">Destination image path. The image format is inferred from the extension.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateWiFiAsync(string ssid, string password, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Wifi(ssid, password, "WPA", false);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.H, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Creates a QR code that opens WhatsApp with a pre-filled message asynchronously.
    /// </summary>
    /// <param name="message">Message text to pre-fill.</param>
    /// <param name="filePath">Destination path for the QR image.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateWhatsAppMessageAsync(string message, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.WhatsAppMessage(message);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code that opens a bookmark in the browser asynchronously.
    /// </summary>
    /// <param name="bookmarkUrl">URL of the bookmark.</param>
    /// <param name="bookmarkName">Bookmark display name.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the QR code should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateBookmarkAsync(string bookmarkUrl, string bookmarkName, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Bookmark(bookmarkUrl, bookmarkName);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
    }

    /// <summary>
    /// Creates a QR code for a calendar event using the iCalendar specification.
    /// </summary>
    /// <para>This helper creates a calendar payload that can be scanned by calendar-aware mobile applications.</para>
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
    /// <example>
    ///   <code>QrCode.GenerateCalendarEvent("Meeting", "Project sync", "Warsaw", DateTime.Today, DateTime.Today.AddHours(1), "meeting.png", false);</code>
    /// </example>
    public static void GenerateCalendarEvent(string calendarEntry, string? calendarMessage, string? calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, QrCalendarEncoding calendarEventEncoding = QrCalendarEncoding.ICalComplete, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20) {
        var payload = QrPayloads.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
    }

    /// <summary>
    /// Creates a QR code for a calendar event using the iCalendar specification asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateCalendarEventAsync(string calendarEntry, string? calendarMessage, string? calendarGeoLocation, DateTime calendarFrom, DateTime calendarTo, string filePath, bool allDayEvent, QrCalendarEncoding calendarEventEncoding = QrCalendarEncoding.ICalComplete, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.CalendarEvent(calendarEntry, calendarMessage, calendarGeoLocation, calendarFrom, calendarTo, allDayEvent, calendarEventEncoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code containing contact information in vCard format asynchronously.
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
    /// <param name="addressType">Address type used in the contact payload.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateContactAsync(string filePath, QrContactOutputType outputType, string firstname, string lastname, string? nickname = null, string? phone = null, string? mobilePhone = null, string? workPhone = null, string? email = null, DateTime? birthday = null, string? website = null, string? street = null, string? houseNumber = null, string? city = null, string? zipCode = null, string? country = null, string? note = null, string? stateRegion = null, QrContactAddressOrder addressOrder = QrContactAddressOrder.Default, string? org = null, string? orgTitle = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, QrContactAddressType addressType = QrContactAddressType.HomePreferred, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Contact(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle, addressType);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code that opens an email draft asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateEmailAsync(string filePath, string email, string? subject = null, string? message = null, QrMailEncoding encoding = QrMailEncoding.Mailto, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Email(email, subject, message, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code that opens the messaging app with an MMS draft asynchronously.
    /// </summary>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="phoneNumber">Recipient phone number.</param>
    /// <param name="subject">Subject line for the MMS message.</param>
    /// <param name="encoding">MMS encoding scheme.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateMMSAsync(string filePath, string phoneNumber, string? subject = null, QrMmsEncoding encoding = QrMmsEncoding.Mmsto, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Mms(phoneNumber, subject, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Creates a QR code that launches the SMS application asynchronously.
    /// </summary>
    /// <param name="number">Phone number to message.</param>
    /// <param name="message">Optional message body.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="encoding">SMS encoding type.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateSmsAsync(string number, string? message, string filePath, QrSmsEncoding encoding = QrSmsEncoding.Sms, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Sms(number, message, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code representing a geographic location asynchronously.
    /// </summary>
    /// <param name="latitude">Latitude coordinate.</param>
    /// <param name="longitude">Longitude coordinate.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="encoding">Geolocation encoding scheme.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateGeoLocationAsync(string latitude, string longitude, string filePath, QrGeolocationEncoding encoding = QrGeolocationEncoding.Geo, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Geo(latitude, longitude, encoding);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Creates a Girocode compliant QR code for SEPA credit transfers asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateGirocodeAsync(string iban, string bic, string name, decimal amount, string filePath, string? remittanceInformation = null, QrGirocodeRemittanceType type = QrGirocodeRemittanceType.Unstructured, string? purposeOfCreditTransfer = null, string? messageToGirocodeUser = null, QrGirocodeVersion version = QrGirocodeVersion.Version1, QrGirocodeEncoding encoding = QrGirocodeEncoding.Iso8859_1, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Girocode(iban, bic, name, amount, remittanceInformation ?? string.Empty, type, purposeOfCreditTransfer ?? string.Empty, messageToGirocodeUser ?? string.Empty, version, encoding);
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code for Bitcoin-like cryptocurrency payments asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateBitcoinAddressAsync(QrBitcoinLikeType currency, string address, double? amount, string? label, string? message, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.BitcoinLike(currency, address, amount, label, message);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code for a Monero transfer asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateMoneroTransactionAsync(string address, float? amount, string? paymentId, string? recipientName, string? description, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Monero(address, amount, paymentId, recipientName, description);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code that initiates a phone call asynchronously.
    /// </summary>
    /// <param name="number">Phone number to call.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GeneratePhoneNumberAsync(string number, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.Phone(number);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code that starts a Skype call asynchronously.
    /// </summary>
    /// <param name="username">Skype username.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should have transparent background.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateSkypeCallAsync(string username, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.SkypeCall(username);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code containing Shadowsocks configuration settings asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateShadowSocksAsync(string hostname, int port, string password, QrShadowSocksMethod method, string filePath, string? tag = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.ShadowSocks(hostname, port, password, method, tag);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a QR code for configuring a one-time password generator asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateOneTimePasswordAsync(OtpAuthType type, string secretBase32, string filePath, string? label = null, string? issuer = null, OtpAlgorithm algorithm = OtpAlgorithm.Sha1, int digits = 6, int? period = 30, int? counter = null, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = QrPayloads.OneTimePassword(type, secretBase32, label, issuer, algorithm, digits, period, counter);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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
    /// Generates a Slovenian UPN QR payment code asynchronously.
    /// </summary>
    /// <param name="upn">Payload with Slovenian UPN payment details.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateSlovenianUpnQrAsync(SlovenianUpnQrPayload upn, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        if (upn is null) {
            throw new ArgumentNullException(nameof(upn));
        }
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(upn.ToPayloadData(), filePath, options, cancellationToken);
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
        var payload = BuildBezahlPayloadData(authority, name, account, bnc, iban, bic, reason, null, "EUR", null, null, null, null, null, QrBezahlPeriodicUnit.Monthly, null, null, null);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
    }

    /// <summary>
    /// Generates a German BezahlCode payment QR code with authority-specific payment options.
    /// </summary>
    /// <param name="authority">Authority type.</param>
    /// <param name="name">Creditor name.</param>
    /// <param name="account">Account number for non-SEPA authorities.</param>
    /// <param name="bnc">Bank number for non-SEPA authorities.</param>
    /// <param name="iban">IBAN of the creditor for SEPA authorities.</param>
    /// <param name="bic">BIC of the creditor for SEPA authorities.</param>
    /// <param name="reason">Payment reason.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="amount">Payment amount for payment authorities.</param>
    /// <param name="currency">Currency code used for payment authorities.</param>
    /// <param name="postingKey">Posting key for non-SEPA payment authorities.</param>
    /// <param name="executionDate">Execution date for single payment or direct debit authorities.</param>
    /// <param name="sepaReference">SEPA reference for SEPA payment authorities.</param>
    /// <param name="creditorId">Creditor identifier for direct debit authorities.</param>
    /// <param name="mandateId">Mandate identifier for direct debit authorities.</param>
    /// <param name="dateOfSignature">Mandate signature date for direct debit authorities.</param>
    /// <param name="periodicUnit">Periodic unit for periodic payment authorities.</param>
    /// <param name="periodicUnitRotation">Periodic unit rotation for periodic payment authorities.</param>
    /// <param name="periodicFirstExecutionDate">First execution date for periodic payment authorities.</param>
    /// <param name="periodicLastExecutionDate">Last execution date for periodic payment authorities.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    public static void GenerateBezahlCode(
        QrBezahlAuthorityType authority,
        string name,
        string account,
        string bnc,
        string iban,
        string bic,
        string reason,
        string filePath,
        decimal? amount,
        string currency = "EUR",
        string? postingKey = null,
        DateTime? executionDate = null,
        string? sepaReference = null,
        string? creditorId = null,
        string? mandateId = null,
        DateTime? dateOfSignature = null,
        QrBezahlPeriodicUnit periodicUnit = QrBezahlPeriodicUnit.Monthly,
        int? periodicUnitRotation = null,
        DateTime? periodicFirstExecutionDate = null,
        DateTime? periodicLastExecutionDate = null,
        bool transparent = false,
        Color? foregroundColor = null,
        Color? backgroundColor = null,
        int pixelSize = 20) {
        var payload = BuildBezahlPayloadData(authority, name, account, bnc, iban, bic, reason, amount, currency, postingKey, executionDate, sepaReference, creditorId, mandateId, periodicUnit, periodicUnitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, dateOfSignature);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        RenderToFile(payload, filePath, options);
    }

    /// <summary>
    /// Generates a German BezahlCode payment QR code asynchronously.
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
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateBezahlCodeAsync(QrBezahlAuthorityType authority, string name, string account, string bnc, string iban, string bic, string reason, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        var payload = BuildBezahlPayloadData(authority, name, account, bnc, iban, bic, reason, null, "EUR", null, null, null, null, null, QrBezahlPeriodicUnit.Monthly, null, null, null);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
    }

    /// <summary>
    /// Generates a German BezahlCode payment QR code asynchronously with authority-specific payment options.
    /// </summary>
    /// <param name="authority">Authority type.</param>
    /// <param name="name">Creditor name.</param>
    /// <param name="account">Account number for non-SEPA authorities.</param>
    /// <param name="bnc">Bank number for non-SEPA authorities.</param>
    /// <param name="iban">IBAN of the creditor for SEPA authorities.</param>
    /// <param name="bic">BIC of the creditor for SEPA authorities.</param>
    /// <param name="reason">Payment reason.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="amount">Payment amount for payment authorities.</param>
    /// <param name="currency">Currency code used for payment authorities.</param>
    /// <param name="postingKey">Posting key for non-SEPA payment authorities.</param>
    /// <param name="executionDate">Execution date for single payment or direct debit authorities.</param>
    /// <param name="sepaReference">SEPA reference for SEPA payment authorities.</param>
    /// <param name="creditorId">Creditor identifier for direct debit authorities.</param>
    /// <param name="mandateId">Mandate identifier for direct debit authorities.</param>
    /// <param name="dateOfSignature">Mandate signature date for direct debit authorities.</param>
    /// <param name="periodicUnit">Periodic unit for periodic payment authorities.</param>
    /// <param name="periodicUnitRotation">Periodic unit rotation for periodic payment authorities.</param>
    /// <param name="periodicFirstExecutionDate">First execution date for periodic payment authorities.</param>
    /// <param name="periodicLastExecutionDate">Last execution date for periodic payment authorities.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateBezahlCodeAsync(
        QrBezahlAuthorityType authority,
        string name,
        string account,
        string bnc,
        string iban,
        string bic,
        string reason,
        string filePath,
        decimal? amount,
        string currency = "EUR",
        string? postingKey = null,
        DateTime? executionDate = null,
        string? sepaReference = null,
        string? creditorId = null,
        string? mandateId = null,
        DateTime? dateOfSignature = null,
        QrBezahlPeriodicUnit periodicUnit = QrBezahlPeriodicUnit.Monthly,
        int? periodicUnitRotation = null,
        DateTime? periodicFirstExecutionDate = null,
        DateTime? periodicLastExecutionDate = null,
        bool transparent = false,
        Color? foregroundColor = null,
        Color? backgroundColor = null,
        int pixelSize = 20,
        CancellationToken cancellationToken = default) {
        var payload = BuildBezahlPayloadData(authority, name, account, bnc, iban, bic, reason, amount, currency, postingKey, executionDate, sepaReference, creditorId, mandateId, periodicUnit, periodicUnitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, dateOfSignature);
        var options = BuildOptions(transparent, QrErrorCorrectionLevel.Q, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(payload, filePath, options, cancellationToken);
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

    /// <summary>
    /// Generates a Swiss QR invoice code asynchronously.
    /// </summary>
    /// <param name="swiss">Swiss QR code payload.</param>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="transparent">Whether the background should be transparent.</param>
    /// <param name="foregroundColor">Foreground color of QR modules.</param>
    /// <param name="backgroundColor">Background color of the QR code.</param>
    /// <param name="pixelSize">Pixel size for each QR module.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image generation or save operations.</param>
    public static Task GenerateSwissQrCodeAsync(SwissQrCodePayload swiss, string filePath, bool transparent = false, Color? foregroundColor = null, Color? backgroundColor = null, int pixelSize = 20, CancellationToken cancellationToken = default) {
        if (swiss is null) {
            throw new ArgumentNullException(nameof(swiss));
        }
        var options = BuildOptions(transparent, null, foregroundColor, backgroundColor, pixelSize);
        return RenderToFileAsync(swiss.ToPayloadData(), filePath, options, cancellationToken);
    }

    private static QrPayloadData BuildBezahlPayloadData(
        QrBezahlAuthorityType authority,
        string name,
        string account,
        string bnc,
        string iban,
        string bic,
        string reason,
        decimal? amount,
        string currency,
        string? postingKey,
        DateTime? executionDate,
        string? sepaReference,
        string? creditorId,
        string? mandateId,
        QrBezahlPeriodicUnit periodicUnit,
        int? periodicUnitRotation,
        DateTime? periodicFirstExecutionDate,
        DateTime? periodicLastExecutionDate,
        DateTime? dateOfSignature = null) {
        return authority switch {
            QrBezahlAuthorityType.Contact or QrBezahlAuthorityType.ContactV2 => QrPayloads.BezahlCode(authority, name, account, bnc, iban, bic, reason),
            QrBezahlAuthorityType.SinglePayment => QrPayloads.BezahlCodeSinglePayment(name, account, bnc, RequireAmount(amount), reason, currency, postingKey ?? string.Empty, executionDate),
            QrBezahlAuthorityType.SinglePaymentSepa => QrPayloads.BezahlCodeSinglePaymentSepa(name, iban, bic, RequireAmount(amount), reason, currency, sepaReference ?? string.Empty, executionDate),
            QrBezahlAuthorityType.SingleDirectDebit => QrPayloads.BezahlCodeSingleDirectDebit(name, account, bnc, RequireAmount(amount), RequireValue(creditorId, nameof(creditorId)), RequireValue(mandateId, nameof(mandateId)), RequireDate(dateOfSignature, nameof(dateOfSignature)), reason, currency, postingKey ?? string.Empty, executionDate),
            QrBezahlAuthorityType.SingleDirectDebitSepa => QrPayloads.BezahlCodeSingleDirectDebitSepa(name, iban, bic, RequireAmount(amount), RequireValue(creditorId, nameof(creditorId)), RequireValue(mandateId, nameof(mandateId)), RequireDate(dateOfSignature, nameof(dateOfSignature)), reason, currency, sepaReference ?? string.Empty, executionDate),
            QrBezahlAuthorityType.PeriodicSinglePayment => QrPayloads.BezahlCodePeriodicSinglePayment(name, account, bnc, RequireAmount(amount), periodicUnit, RequireValue(periodicUnitRotation, nameof(periodicUnitRotation)), RequireDate(periodicFirstExecutionDate, nameof(periodicFirstExecutionDate)), RequireDate(periodicLastExecutionDate, nameof(periodicLastExecutionDate)), reason, currency, postingKey ?? string.Empty),
            QrBezahlAuthorityType.PeriodicSinglePaymentSepa => QrPayloads.BezahlCodePeriodicSinglePaymentSepa(name, iban, bic, RequireAmount(amount), periodicUnit, RequireValue(periodicUnitRotation, nameof(periodicUnitRotation)), RequireDate(periodicFirstExecutionDate, nameof(periodicFirstExecutionDate)), RequireDate(periodicLastExecutionDate, nameof(periodicLastExecutionDate)), reason, currency, sepaReference ?? string.Empty),
            _ => throw new ArgumentOutOfRangeException(nameof(authority), authority, "Unsupported BezahlCode authority.")
        };
    }

    private static decimal RequireAmount(decimal? amount) {
        if (!amount.HasValue) {
            throw new ArgumentNullException(nameof(amount), "Amount is required for the selected BezahlCode authority.");
        }

        return amount.Value;
    }

    private static string RequireValue(string? value, string paramName) {
        if (string.IsNullOrWhiteSpace(value)) {
            throw new ArgumentException($"Value is required for the selected BezahlCode authority.", paramName);
        }

        return value!;
    }

    private static int RequireValue(int? value, string paramName) {
        if (!value.HasValue) {
            throw new ArgumentNullException(paramName, "Value is required for the selected BezahlCode authority.");
        }

        return value.Value;
    }

    private static DateTime RequireDate(DateTime? value, string paramName) {
        if (!value.HasValue) {
            throw new ArgumentNullException(paramName, "Value is required for the selected BezahlCode authority.");
        }

        return value.Value;
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

    private static string ResolveValidatedOutputPath(string filePath, out string extension) {
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        extension = Path.GetExtension(fullPath);
        if (string.IsNullOrWhiteSpace(extension)) {
            throw new UnknownImageFormatException(
                $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}");
        }

        return fullPath;
    }

    private static void RenderToFile(QrPayloadData payload, string filePath, QrEasyOptions options) {
        string fullPath = ResolveValidatedOutputPath(filePath, out _);
        CodeGlyphX.QrCode.Save(payload, fullPath, options);
    }

    private static async Task RenderToFileAsync(QrPayloadData payload, string filePath, QrEasyOptions options, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = ResolveValidatedOutputPath(filePath, out _);
        byte[] pngBytes = CodeGlyphX.QrCode.Render(payload, CodeGlyphX.Rendering.OutputFormat.Png, options).Data;
        cancellationToken.ThrowIfCancellationRequested();
        await SavePngBytesAsync(pngBytes, fullPath, cancellationToken).ConfigureAwait(false);
    }

    private static void RenderToFileWithCenteredLogo(QrPayloadData payload, string filePath, string logoPath, QrEasyOptions options) {
        string fullPath = Helpers.ResolvePath(filePath);
        string extension = Path.GetExtension(fullPath);

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                RenderToFile(payload, tempPath, options);
                OverlayCenteredLogo(tempPath, logoPath, fullPath);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }

            return;
        }

        RenderToFile(payload, fullPath, options);
        OverlayCenteredLogo(fullPath, logoPath, fullPath);
    }

    private static async Task RenderToFileWithCenteredLogoAsync(QrPayloadData payload, string filePath, string logoPath, QrEasyOptions options, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        string extension = Path.GetExtension(fullPath);

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                await RenderToFileAsync(payload, tempPath, options, cancellationToken).ConfigureAwait(false);
                await OverlayCenteredLogoAsync(tempPath, logoPath, fullPath, cancellationToken).ConfigureAwait(false);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }

            return;
        }

        await RenderToFileAsync(payload, fullPath, options, cancellationToken).ConfigureAwait(false);
        await OverlayCenteredLogoAsync(fullPath, logoPath, fullPath, cancellationToken).ConfigureAwait(false);
    }

    private static byte[] LoadLogoPng(string logoPath) {
        string fullLogoPath = Helpers.ResolvePath(logoPath);
        using Image<Rgba32> logo = SixLabors.ImageSharp.Image.Load<Rgba32>(fullLogoPath);
        using MemoryStream ms = new();
        logo.SaveAsPng(ms);
        return ms.ToArray();
    }

    private static void OverlayCenteredLogo(string qrPath, string logoPath, string outputPath) {
        string fullQrPath = Helpers.ResolvePath(qrPath);
        string fullLogoPath = Helpers.ResolvePath(logoPath);
        string fullOutputPath = Helpers.ResolvePath(outputPath);

#if NET472
        OverlayCenteredLogoFramework(fullQrPath, fullLogoPath, fullOutputPath);
        return;
#else
        string tempOutputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{Path.GetExtension(fullOutputPath)}");

        try {
            using (Image<Rgba32> qrImage = SixLabors.ImageSharp.Image.Load<Rgba32>(fullQrPath))
            using (Image<Rgba32> logoImage = SixLabors.ImageSharp.Image.Load<Rgba32>(fullLogoPath)) {
                int maxLogoWidth = Math.Max(1, qrImage.Width / 5);
                int maxLogoHeight = Math.Max(1, qrImage.Height / 5);
                double widthRatio = maxLogoWidth / (double)logoImage.Width;
                double heightRatio = maxLogoHeight / (double)logoImage.Height;
                double scale = Math.Min(widthRatio, heightRatio);
                int logoWidth = Math.Max(1, (int)Math.Round(logoImage.Width * scale));
                int logoHeight = Math.Max(1, (int)Math.Round(logoImage.Height * scale));

                logoImage.Mutate(ctx => ctx.Resize(new ResizeOptions {
                    Mode = ResizeMode.Max,
                    Size = new Size(logoWidth, logoHeight)
                }));

                int x = (qrImage.Width - logoImage.Width) / 2;
                int y = (qrImage.Height - logoImage.Height) / 2;
                qrImage.Mutate(ctx => ctx.DrawImage(logoImage, new Point(x, y), 1f));

                SaveCompositeImage(qrImage, tempOutputPath);
            }

            File.Copy(tempOutputPath, fullOutputPath, true);
        } finally {
            if (File.Exists(tempOutputPath)) {
                File.Delete(tempOutputPath);
            }
        }
#endif
    }

    private static async Task OverlayCenteredLogoAsync(string qrPath, string logoPath, string outputPath, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullQrPath = Helpers.ResolvePath(qrPath);
        string fullLogoPath = Helpers.ResolvePath(logoPath);
        string fullOutputPath = Helpers.ResolvePath(outputPath);

#if NET472
        OverlayCenteredLogoFramework(fullQrPath, fullLogoPath, fullOutputPath);
        await Task.CompletedTask.ConfigureAwait(false);
#else
        string tempOutputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{Path.GetExtension(fullOutputPath)}");

        try {
            using (Image<Rgba32> qrImage = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(fullQrPath, cancellationToken).ConfigureAwait(false))
            using (Image<Rgba32> logoImage = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(fullLogoPath, cancellationToken).ConfigureAwait(false)) {
                cancellationToken.ThrowIfCancellationRequested();
                int maxLogoWidth = Math.Max(1, qrImage.Width / 5);
                int maxLogoHeight = Math.Max(1, qrImage.Height / 5);
                double widthRatio = maxLogoWidth / (double)logoImage.Width;
                double heightRatio = maxLogoHeight / (double)logoImage.Height;
                double scale = Math.Min(widthRatio, heightRatio);
                int logoWidth = Math.Max(1, (int)Math.Round(logoImage.Width * scale));
                int logoHeight = Math.Max(1, (int)Math.Round(logoImage.Height * scale));

                logoImage.Mutate(ctx => ctx.Resize(new ResizeOptions {
                    Mode = ResizeMode.Max,
                    Size = new Size(logoWidth, logoHeight)
                }));

                int x = (qrImage.Width - logoImage.Width) / 2;
                int y = (qrImage.Height - logoImage.Height) / 2;
                qrImage.Mutate(ctx => ctx.DrawImage(logoImage, new Point(x, y), 1f));

                await SaveCompositeImageAsync(qrImage, tempOutputPath, cancellationToken).ConfigureAwait(false);
            }

            File.Copy(tempOutputPath, fullOutputPath, true);
        } finally {
            if (File.Exists(tempOutputPath)) {
                File.Delete(tempOutputPath);
            }
        }
#endif
    }

    #if NET472
    private static void OverlayCenteredLogoFramework(string qrPath, string logoPath, string outputPath) {
        string tempOutputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{Path.GetExtension(outputPath)}");
        try {
            using (var sourceBitmap = new System.Drawing.Bitmap(qrPath))
            using (var qrBitmap = new System.Drawing.Bitmap(sourceBitmap.Width, sourceBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            using (var logoBitmap = new System.Drawing.Bitmap(logoPath)) {
                using (var graphics = System.Drawing.Graphics.FromImage(qrBitmap)) {
                    graphics.DrawImage(sourceBitmap, 0, 0, sourceBitmap.Width, sourceBitmap.Height);
                }

                int maxLogoWidth = Math.Max(1, qrBitmap.Width / 5);
                int maxLogoHeight = Math.Max(1, qrBitmap.Height / 5);
                double widthRatio = maxLogoWidth / (double)logoBitmap.Width;
                double heightRatio = maxLogoHeight / (double)logoBitmap.Height;
                double scale = Math.Min(widthRatio, heightRatio);
                int logoWidth = Math.Max(1, (int)Math.Round(logoBitmap.Width * scale));
                int logoHeight = Math.Max(1, (int)Math.Round(logoBitmap.Height * scale));
                int x = (qrBitmap.Width - logoWidth) / 2;
                int y = (qrBitmap.Height - logoHeight) / 2;

                using (var graphics = System.Drawing.Graphics.FromImage(qrBitmap))
                using (var resizedLogo = new System.Drawing.Bitmap(logoBitmap, new System.Drawing.Size(logoWidth, logoHeight))) {
                    graphics.DrawImage(resizedLogo, x, y, logoWidth, logoHeight);
                }

                SaveCompositeBitmapFramework(qrBitmap, tempOutputPath);
            }

            File.Copy(tempOutputPath, outputPath, true);
        } finally {
            if (File.Exists(tempOutputPath)) {
                File.Delete(tempOutputPath);
            }
        }
    }

    private static void SaveCompositeBitmapFramework(System.Drawing.Bitmap bitmap, string filePath) {
        Helpers.CreateParentDirectory(filePath);
        string extension = Path.GetExtension(filePath);

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPngPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                bitmap.Save(tempPngPath, System.Drawing.Imaging.ImageFormat.Png);
                using var wrappedImage = Image.Load(tempPngPath);
                wrappedImage.SaveAsIcon(filePath);
            } finally {
                if (File.Exists(tempPngPath)) {
                    File.Delete(tempPngPath);
                }
            }
            return;
        }

        bitmap.Save(filePath, GetFrameworkImageFormat(extension));
    }

    private static System.Drawing.Imaging.ImageFormat GetFrameworkImageFormat(string extension) {
        return extension.ToLowerInvariant() switch {
            ".png" => System.Drawing.Imaging.ImageFormat.Png,
            ".jpg" => System.Drawing.Imaging.ImageFormat.Jpeg,
            ".jpeg" => System.Drawing.Imaging.ImageFormat.Jpeg,
            ".bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
            ".gif" => System.Drawing.Imaging.ImageFormat.Gif,
            ".tiff" => System.Drawing.Imaging.ImageFormat.Tiff,
            _ => System.Drawing.Imaging.ImageFormat.Png
        };
    }
    #endif

    private static void SaveCompositeImage(Image<Rgba32> image, string filePath) {
        Helpers.CreateParentDirectory(filePath);
        string extension = Path.GetExtension(filePath);

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                image.SaveAsPng(tempPath);
                using var wrappedImage = Image.Load(tempPath);
                wrappedImage.SaveAsIcon(filePath);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }
            return;
        }

        image.Save(filePath, Helpers.GetEncoder(extension, null, null));
    }

    private static async Task SavePngBytesAsync(byte[] pngBytes, string filePath, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = ResolveValidatedOutputPath(filePath, out string extension);

        if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            await WriteAllBytesAsync(fullPath, pngBytes, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                await WriteAllBytesAsync(tempPath, pngBytes, cancellationToken).ConfigureAwait(false);
                using var wrappedImage = Image.Load(tempPath);
                wrappedImage.SaveAsIcon(fullPath);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }
            return;
        }

        using MemoryStream input = new(pngBytes, writable: false);
        using Image<Rgba32> image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(input, cancellationToken).ConfigureAwait(false);
        using FileStream output = File.Create(fullPath);
        await image.SaveAsync(output, Helpers.GetEncoder(extension, null, null), cancellationToken).ConfigureAwait(false);
    }

    private static async Task SaveCompositeImageAsync(Image<Rgba32> image, string filePath, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        Helpers.CreateParentDirectory(filePath);
        string extension = Path.GetExtension(filePath);

        if (extension.Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                using FileStream tempOutput = File.Create(tempPath);
                await image.SaveAsPngAsync(tempOutput, cancellationToken).ConfigureAwait(false);
                using var wrappedImage = Image.Load(tempPath);
                wrappedImage.SaveAsIcon(filePath);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }
            return;
        }

        using FileStream output = File.Create(filePath);
        await image.SaveAsync(output, Helpers.GetEncoder(extension, null, null), cancellationToken).ConfigureAwait(false);
    }

    private static async Task WriteAllBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken) {
        using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);
        await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<byte[]> ReadAllBytesAsync(string filePath, CancellationToken cancellationToken) {
        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
        if (stream.Length > int.MaxValue) {
            throw new IOException($"File is too large to read into memory: {filePath}");
        }

        byte[] buffer = new byte[stream.Length];
        int offset = 0;
        while (offset < buffer.Length) {
            int read = await stream.ReadAsync(buffer, offset, buffer.Length - offset, cancellationToken).ConfigureAwait(false);
            if (read == 0) {
                break;
            }

            offset += read;
        }

        if (offset == buffer.Length) {
            return buffer;
        }

        byte[] result = new byte[offset];
        Buffer.BlockCopy(buffer, 0, result, 0, offset);
        return result;
    }

    /// <summary>
    /// Reads a QR code image and returns the decoded payload.
    /// </summary>
    /// <para>The returned result reports whether a QR code was found and includes the decoded payload text when successful.</para>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <returns>Decoded barcode result.</returns>
    /// <example>
    ///   <code>var result = QrCode.Read("code.png");</code>
    /// </example>
    public static BarcodeResult<Rgba32> Read(string filePath) {
        return ReadAsync(filePath).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Reads a QR code image and returns the decoded payload asynchronously.
    /// </summary>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image loading or fallback reads.</param>
    /// <returns>Decoded barcode result.</returns>
    public static async Task<BarcodeResult<Rgba32>> ReadAsync(string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        #if NET8_0_OR_GREATER
        using Image<Rgba32> image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(fullPath, cancellationToken).ConfigureAwait(false);
        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);

        cancellationToken.ThrowIfCancellationRequested();
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

        decoded = await TryDecodeImageFallbackAsync(fullPath, cancellationToken).ConfigureAwait(false);
        if (decoded is not null) {
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
        var decoded = await TryDecodeImageFallbackAsync(fullPath, cancellationToken).ConfigureAwait(false);
        if (decoded is not null) {
            return new BarcodeResult<Rgba32> {
                Status = Status.Found,
                Message = decoded.Text,
                Value = decoded.Text
            };
        }

        return new BarcodeResult<Rgba32> {
            Status = Status.NotFound
        };
        #endif

    }

    private static async Task<QrDecoded?> TryDecodeImageFallbackAsync(string fullPath, CancellationToken cancellationToken) {
        cancellationToken.ThrowIfCancellationRequested();
        byte[] imageBytes = await ReadAllBytesAsync(fullPath, cancellationToken).ConfigureAwait(false);
        if (QrImageDecoder.TryDecodeImage(imageBytes, out var decoded)) {
            return decoded;
        }

        var aggressiveOptions = new QrPixelDecodeOptions {
            Profile = QrDecodeProfile.Robust,
            AggressiveSampling = true,
            StylizedSampling = true
        };

        cancellationToken.ThrowIfCancellationRequested();
        if (QrImageDecoder.TryDecodeImage(imageBytes, aggressiveOptions, out decoded)) {
            return decoded;
        }

        using FileStream stream = File.OpenRead(fullPath);
        if (QrImageDecoder.TryDecodeImage(stream, out decoded)) {
            return decoded;
        }

        cancellationToken.ThrowIfCancellationRequested();
        stream.Position = 0;
        return QrImageDecoder.TryDecodeImage(stream, aggressiveOptions, out decoded) ? decoded : null;
    }

    #if NET8_0_OR_GREATER
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




