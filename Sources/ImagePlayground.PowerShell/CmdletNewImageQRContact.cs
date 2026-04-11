using ImagePlayground;
using System;
using System.Management.Automation;
using CodeGlyphX.Payloads;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Generates a QR code image containing the provided contact details.
/// </summary>
/// <example>
///   <summary>Create a vCard QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRContact -FilePath contact.png -Firstname John -Lastname Doe -Phone 123456789</code>
///   <para>Creates a simple contact QR code that can be scanned into a phone address book.</para>
/// </example>
/// <example>
///   <summary>Create a richer business contact QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRContact -FilePath consultant.png -OutputType VCard4 -Firstname John -Lastname Doe -Email john.doe@evotec.pl -MobilePhone '+48 500 600 700' -Org Evotec -OrgTitle Consultant -Street Example -HouseNumber 15A -City Warsaw -ZipCode '00-001' -Country Poland -Website 'https://evotec.pl'</code>
///   <para>Generates a business card style QR code with address, organization, and contact metadata.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRContact")]
public sealed class NewImageQrContactCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Output file path.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Contact output type.</summary>
    [Parameter]
    public QrContactOutputType OutputType { get; set; } = QrContactOutputType.VCard4;

    /// <summary>Given name of the contact.</summary>
    [Parameter]
    public string? Firstname { get; set; }

    /// <summary>Surname of the contact.</summary>
    [Parameter]
    public string? Lastname { get; set; }

    /// <summary>Optional nickname.</summary>
    [Parameter]
    public string? Nickname { get; set; }

    /// <summary>Primary phone number.</summary>
    [Parameter]
    public string? Phone { get; set; }

    /// <summary>Mobile phone number.</summary>
    [Parameter]
    public string? MobilePhone { get; set; }

    /// <summary>Work phone number.</summary>
    [Parameter]
    public string? WorkPhone { get; set; }

    /// <summary>Email address.</summary>
    [Parameter]
    public string? Email { get; set; }

    /// <summary>Birthday date.</summary>
    [Parameter]
    public DateTime? Birthday { get; set; }

    /// <summary>Personal or company website.</summary>
    [Parameter]
    public string? Website { get; set; }

    /// <summary>Street name of the address.</summary>
    [Parameter]
    public string? Street { get; set; }

    /// <summary>House or building number.</summary>
    [Parameter]
    public string? HouseNumber { get; set; }

    /// <summary>City name.</summary>
    [Parameter]
    public string? City { get; set; }

    /// <summary>Postal code.</summary>
    [Parameter]
    public string? ZipCode { get; set; }

    /// <summary>Country name.</summary>
    [Parameter]
    public string? Country { get; set; }

    /// <summary>Additional notes.</summary>
    [Parameter]
    public string? Note { get; set; }

    /// <summary>State or region.</summary>
    [Parameter]
    public string? StateRegion { get; set; }
    /// <summary>Order of address fields in the QR code.</summary>
    [Parameter]
    public QrContactAddressOrder AddressOrder { get; set; } = QrContactAddressOrder.Default;

    /// <summary>Organization name.</summary>
    [Parameter]
    public string? Org { get; set; }

    /// <summary>Contact's title or role within the organization.</summary>
    [Parameter]
    public string? OrgTitle { get; set; }

    /// <summary>Open image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Foreground color of QR modules.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color ForegroundColor { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Background color of the QR code.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color BackgroundColor { get; set; } = SixLabors.ImageSharp.Color.White;

    /// <summary>Pixel size for each QR module.</summary>
    [Parameter]
    public int PixelSize { get; set; } = 20;

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateContactAsync(FilePath, OutputType, Firstname ?? string.Empty, Lastname ?? string.Empty, Nickname, Phone, MobilePhone, WorkPhone, Email, Birthday, Website, Street, HouseNumber, City, ZipCode, Country, Note, StateRegion, AddressOrder, Org, OrgTitle, false, ForegroundColor, BackgroundColor, PixelSize, QrContactAddressType.HomePreferred, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateContact(FilePath, OutputType, Firstname ?? string.Empty, Lastname ?? string.Empty, Nickname, Phone, MobilePhone, WorkPhone, Email, Birthday, Website, Street, HouseNumber, City, ZipCode, Country, Note, StateRegion, AddressOrder, Org, OrgTitle, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}
