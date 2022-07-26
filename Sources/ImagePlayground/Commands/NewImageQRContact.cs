using System;
using System.IO;
using System.Management.Automation;
using QRCoder;

namespace ImagePlayground.PowerShell;

[Cmdlet(VerbsCommon.New, "ImageQRContact")]
[OutputType(typeof(void))]
public class NewImageQRContact : PSCmdlet
{
    [Parameter(
        Mandatory = true
    )]
    [AllowNull]
    [AllowEmptyString]
    public string FilePath { get; set; }

    [Parameter()]
    public PayloadGenerator.ContactData.ContactOutputType OutputType { get; set; } = PayloadGenerator.ContactData.ContactOutputType.VCard4;

    [Parameter()]
    public string FirstName { get; set; }

    [Parameter()]
    public string LastName { get; set; }

    [Parameter()]
    public string NickName { get; set; } = null;

    [Parameter()]
    public string Phone { get; set; } = null;

    [Parameter()]
    public string MobilePhone { get; set; } = null;

    [Parameter()]
    public string WorkPhone { get; set; } = null;

    [Parameter()]
    public string Email { get; set; } = null;

    [Parameter()]
    public DateTime? Birthday { get; set; } = null;

    [Parameter()]
    public string Website { get; set; } = null;

    [Parameter()]
    public string Street { get; set; } = null;

    [Parameter()]
    public string HouseNumber { get; set; } = null;

    [Parameter()]
    public string City { get; set; } = null;

    [Parameter()]
    public string ZipCode { get; set; } = null;

    [Parameter()]
    public string Country { get; set; } = null;

    [Parameter()]
    public string Note { get; set; } = null;

    [Parameter()]
    public string StateRegion { get; set; } = null;

    [Parameter()]
    public PayloadGenerator.ContactData.AddressOrder AddressOrder { get; set; } = PayloadGenerator.ContactData.AddressOrder.Default;

    [Parameter()]
    public string Org { get; set; } = null;

    [Parameter()]
    public string OrgTitle { get; set; } = null;

    [Parameter()]
    public SwitchParameter Show { get; set; }

    protected override void EndProcessing()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            string tmpName = Path.GetRandomFileName().Split('.')[0];
            FilePath = Path.Combine(Path.GetTempPath(), $"{tmpName}.png");
            WriteWarning($"New-ImageQRContact - No file path specified, saving to {FilePath}");
        }

        QrCode.GenerateContact(
            FilePath,
            OutputType,
            FirstName,
            LastName,
            phone: Phone,
            mobilePhone: MobilePhone,
            email: Email,
            birthday: Birthday,
            website: Website,
            street: Street,
            houseNumber: HouseNumber,
            city: City,
            zipCode: ZipCode,
            country: Country,
            note: Note,
            stateRegion: StateRegion,
            addressOrder: AddressOrder,
            org: Org,
            orgTitle: OrgTitle,
            transparent: false
        );

        if (Show)
        {
            WriteError(new ErrorRecord(
                new NotImplementedException("Show not implemented"),
                "NotImplemented",
                ErrorCategory.NotImplemented,
                null));
        }
    }
}