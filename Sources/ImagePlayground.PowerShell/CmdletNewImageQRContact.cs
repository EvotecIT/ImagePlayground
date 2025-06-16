using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code with contact information.</summary>
[Cmdlet(VerbsCommon.New, "ImageQRContact")]
public sealed class NewImageQrContactCmdlet : PSCmdlet {
    /// <summary>Output file path.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Contact output type.</summary>
    [Parameter]
    public QRCoder.PayloadGenerator.ContactData.ContactOutputType OutputType { get; set; } = QRCoder.PayloadGenerator.ContactData.ContactOutputType.VCard4;

    [Parameter] public string? Firstname { get; set; }
    [Parameter] public string? Lastname { get; set; }
    [Parameter] public string? Nickname { get; set; }
    [Parameter] public string? Phone { get; set; }
    [Parameter] public string? MobilePhone { get; set; }
    [Parameter] public string? WorkPhone { get; set; }
    [Parameter] public string? Email { get; set; }
    [Parameter] public DateTime? Birthday { get; set; }
    [Parameter] public string? Website { get; set; }
    [Parameter] public string? Street { get; set; }
    [Parameter] public string? HouseNumber { get; set; }
    [Parameter] public string? City { get; set; }
    [Parameter] public string? ZipCode { get; set; }
    [Parameter] public string? Country { get; set; }
    [Parameter] public string? Note { get; set; }
    [Parameter] public string? StateRegion { get; set; }
    [Parameter]
    public QRCoder.PayloadGenerator.ContactData.AddressOrder AddressOrder { get; set; } = QRCoder.PayloadGenerator.ContactData.AddressOrder.Default;
    [Parameter] public string? Org { get; set; }
    [Parameter] public string? OrgTitle { get; set; }

    /// <summary>Open image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRContact - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateContact(FilePath, OutputType, Firstname ?? string.Empty, Lastname ?? string.Empty, Nickname, Phone, MobilePhone, WorkPhone, Email, Birthday, Website, Street, HouseNumber, City, ZipCode, Country, Note, StateRegion, AddressOrder, Org, OrgTitle, false);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(FilePath, true);
        }
    }
}
