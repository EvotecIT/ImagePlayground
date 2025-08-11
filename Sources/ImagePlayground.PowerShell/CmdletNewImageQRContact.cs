using ImagePlayground;
using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Generates a QR code image containing the provided contact details.
/// </summary>
/// <example>
///   <summary>Create a vCard QR code</summary>
///   <code>New-ImageQRContact -FilePath contact.png -Firstname John -Lastname Doe -Phone 123456789</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRContact")]
public sealed class NewImageQrContactCmdlet : PSCmdlet {
    /// <summary>Output file path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Contact output type.</summary>
    [Parameter]
    public QRCoder.PayloadGenerator.ContactData.ContactOutputType OutputType { get; set; } = QRCoder.PayloadGenerator.ContactData.ContactOutputType.VCard4;

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
    public QRCoder.PayloadGenerator.ContactData.AddressOrder AddressOrder { get; set; } = QRCoder.PayloadGenerator.ContactData.AddressOrder.Default;

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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRContact - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateContact(output, OutputType, Firstname ?? string.Empty, Lastname ?? string.Empty, Nickname, Phone, MobilePhone, WorkPhone, Email, Birthday, Website, Street, HouseNumber, City, ZipCode, Country, Note, StateRegion, AddressOrder, Org, OrgTitle, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}
