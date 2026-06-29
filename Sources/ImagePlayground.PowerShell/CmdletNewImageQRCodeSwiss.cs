using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Swiss QR payment code.</summary>
/// <para>Use this cmdlet to render Swiss QR bill payment details into a payment QR image.</para>
/// <example>
///   <summary>Create a Swiss QR payment code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath swiss.png</code>
///   <para>Renders a Swiss QR payment code from the payment fields supplied to the cmdlet.</para>
/// </example>
/// <example>
///   <summary>Create a Swiss QR code with amount, message, and custom colors</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -Amount 249.99 -UnstructuredMessage 'Invoice 2026-041' -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show</code>
///   <para>Creates a styled Swiss QR payment image and opens it immediately after generation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSwiss")]
public sealed class NewImageQrCodeSwissCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Swiss or Liechtenstein IBAN.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Iban { get; set; } = string.Empty;

    /// <summary>IBAN kind.</summary>
    [Parameter]
    public SwissQrCodePayload.Iban.IbanType IbanType { get; set; } = SwissQrCodePayload.Iban.IbanType.Iban;

    /// <summary>Payment currency.</summary>
    [Parameter]
    public QrSwissCurrency Currency { get; set; } = QrSwissCurrency.CHF;

    /// <summary>Creditor name.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string CreditorName { get; set; } = string.Empty;

    /// <summary>Creditor address type.</summary>
    [Parameter]
    public SwissQrCodePayload.Contact.AddressType CreditorAddressType { get; set; } = SwissQrCodePayload.Contact.AddressType.StructuredAddress;

    /// <summary>Creditor street for structured addresses.</summary>
    [Parameter(Position = 2)]
    public string? CreditorStreet { get; set; }

    /// <summary>Creditor house number for structured addresses.</summary>
    [Parameter(Position = 3)]
    public string? CreditorHouseNumber { get; set; }

    /// <summary>Creditor postal code for structured addresses.</summary>
    [Parameter(Position = 4)]
    public string? CreditorPostalCode { get; set; }

    /// <summary>Creditor city for structured addresses.</summary>
    [Parameter(Position = 5)]
    public string? CreditorCity { get; set; }

    /// <summary>Creditor first address line for combined addresses.</summary>
    [Parameter]
    public string? CreditorAddressLine1 { get; set; }

    /// <summary>Creditor second address line for combined addresses.</summary>
    [Parameter]
    public string? CreditorAddressLine2 { get; set; }

    /// <summary>Creditor two-letter country code.</summary>
    [Parameter]
    public string CreditorCountry { get; set; } = "CH";

    /// <summary>Reference type.</summary>
    [Parameter]
    public SwissQrCodePayload.Reference.ReferenceType ReferenceType { get; set; } = SwissQrCodePayload.Reference.ReferenceType.NON;

    /// <summary>Reference text for QRR or SCOR reference types.</summary>
    [Parameter]
    public string? Reference { get; set; }

    /// <summary>Reference text kind. When omitted, QRR uses QR reference and SCOR uses ISO 11649 creditor reference.</summary>
    [Parameter]
    public SwissQrCodePayload.Reference.ReferenceTextType? ReferenceTextType { get; set; }

    /// <summary>Optional payment amount.</summary>
    [Parameter]
    public decimal? Amount { get; set; }

    /// <summary>Optional unstructured payment message.</summary>
    [Parameter]
    public string? UnstructuredMessage { get; set; }

    /// <summary>Optional bill information.</summary>
    [Parameter]
    public string? BillInformation { get; set; }

    /// <summary>Optional first alternative procedure block.</summary>
    [Parameter]
    public string? AlternativeProcedure1 { get; set; }

    /// <summary>Optional second alternative procedure block.</summary>
    [Parameter]
    public string? AlternativeProcedure2 { get; set; }

    /// <summary>Debtor name.</summary>
    [Parameter]
    public string? DebtorName { get; set; }

    /// <summary>Debtor address type.</summary>
    [Parameter]
    public SwissQrCodePayload.Contact.AddressType DebtorAddressType { get; set; } = SwissQrCodePayload.Contact.AddressType.StructuredAddress;

    /// <summary>Debtor street for structured addresses.</summary>
    [Parameter]
    public string? DebtorStreet { get; set; }

    /// <summary>Debtor house number for structured addresses.</summary>
    [Parameter]
    public string? DebtorHouseNumber { get; set; }

    /// <summary>Debtor postal code for structured addresses.</summary>
    [Parameter]
    public string? DebtorPostalCode { get; set; }

    /// <summary>Debtor city for structured addresses.</summary>
    [Parameter]
    public string? DebtorCity { get; set; }

    /// <summary>Debtor first address line for combined addresses.</summary>
    [Parameter]
    public string? DebtorAddressLine1 { get; set; }

    /// <summary>Debtor second address line for combined addresses.</summary>
    [Parameter]
    public string? DebtorAddressLine2 { get; set; }

    /// <summary>Debtor two-letter country code.</summary>
    [Parameter]
    public string DebtorCountry { get; set; } = "CH";

    /// <summary>Ultimate creditor name.</summary>
    [Parameter]
    public string? UltimateCreditorName { get; set; }

    /// <summary>Ultimate creditor address type.</summary>
    [Parameter]
    public SwissQrCodePayload.Contact.AddressType UltimateCreditorAddressType { get; set; } = SwissQrCodePayload.Contact.AddressType.StructuredAddress;

    /// <summary>Ultimate creditor street for structured addresses.</summary>
    [Parameter]
    public string? UltimateCreditorStreet { get; set; }

    /// <summary>Ultimate creditor house number for structured addresses.</summary>
    [Parameter]
    public string? UltimateCreditorHouseNumber { get; set; }

    /// <summary>Ultimate creditor postal code for structured addresses.</summary>
    [Parameter]
    public string? UltimateCreditorPostalCode { get; set; }

    /// <summary>Ultimate creditor city for structured addresses.</summary>
    [Parameter]
    public string? UltimateCreditorCity { get; set; }

    /// <summary>Ultimate creditor first address line for combined addresses.</summary>
    [Parameter]
    public string? UltimateCreditorAddressLine1 { get; set; }

    /// <summary>Ultimate creditor second address line for combined addresses.</summary>
    [Parameter]
    public string? UltimateCreditorAddressLine2 { get; set; }

    /// <summary>Ultimate creditor two-letter country code.</summary>
    [Parameter]
    public string UltimateCreditorCountry { get; set; } = "CH";

    /// <summary>Path for the generated image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 6)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image once generated.</summary>
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

        var payload = BuildPayload();

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateSwissQrCodeAsync(payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSwissQrCode(payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }

    private SwissQrCodePayload BuildPayload() {
        var iban = new SwissQrCodePayload.Iban(Iban, IbanType);
        var creditor = CreateContact(
            nameof(CreditorName),
            CreditorName,
            CreditorAddressType,
            CreditorStreet,
            CreditorHouseNumber,
            CreditorPostalCode,
            CreditorCity,
            CreditorAddressLine1,
            CreditorAddressLine2,
            CreditorCountry);
        var reference = BuildReference();
        var additionalInformation = new SwissQrCodePayload.AdditionalInformation(UnstructuredMessage, BillInformation);
        var debtor = TryCreateOptionalContact(
            nameof(DebtorName),
            DebtorName,
            DebtorAddressType,
            DebtorStreet,
            DebtorHouseNumber,
            DebtorPostalCode,
            DebtorCity,
            DebtorAddressLine1,
            DebtorAddressLine2,
            DebtorCountry);
        var ultimateCreditor = TryCreateOptionalContact(
            nameof(UltimateCreditorName),
            UltimateCreditorName,
            UltimateCreditorAddressType,
            UltimateCreditorStreet,
            UltimateCreditorHouseNumber,
            UltimateCreditorPostalCode,
            UltimateCreditorCity,
            UltimateCreditorAddressLine1,
            UltimateCreditorAddressLine2,
            UltimateCreditorCountry);

        return new SwissQrCodePayload(iban, Currency, creditor, reference, additionalInformation, debtor, Amount, ultimateCreditor, AlternativeProcedure1, AlternativeProcedure2);
    }

    private SwissQrCodePayload.Reference BuildReference() {
        var referenceTextType = ReferenceTextType;
        if (!referenceTextType.HasValue && ReferenceType == SwissQrCodePayload.Reference.ReferenceType.QRR) {
            referenceTextType = SwissQrCodePayload.Reference.ReferenceTextType.QrReference;
        } else if (!referenceTextType.HasValue && ReferenceType == SwissQrCodePayload.Reference.ReferenceType.SCOR) {
            referenceTextType = SwissQrCodePayload.Reference.ReferenceTextType.CreditorReferenceIso11649;
        }

        return new SwissQrCodePayload.Reference(ReferenceType, Reference, referenceTextType);
    }

    private static SwissQrCodePayload.Contact? TryCreateOptionalContact(
        string nameParameter,
        string? name,
        SwissQrCodePayload.Contact.AddressType addressType,
        string? street,
        string? houseNumber,
        string? postalCode,
        string? city,
        string? addressLine1,
        string? addressLine2,
        string country) {
        if (!HasContactValues(name, street, houseNumber, postalCode, city, addressLine1, addressLine2)) {
            return null;
        }

        return CreateContact(nameParameter, name, addressType, street, houseNumber, postalCode, city, addressLine1, addressLine2, country);
    }

    private static SwissQrCodePayload.Contact CreateContact(
        string nameParameter,
        string? name,
        SwissQrCodePayload.Contact.AddressType addressType,
        string? street,
        string? houseNumber,
        string? postalCode,
        string? city,
        string? addressLine1,
        string? addressLine2,
        string country) {
        var requiredName = RequireValue(name, nameParameter);
        if (addressType == SwissQrCodePayload.Contact.AddressType.CombinedAddress) {
            return SwissQrCodePayload.Contact.CreateCombined(
                requiredName,
                RequireValue(addressLine1, nameParameter.Replace("Name", "AddressLine1")),
                RequireValue(addressLine2, nameParameter.Replace("Name", "AddressLine2")),
                country);
        }

        return SwissQrCodePayload.Contact.CreateStructured(
            requiredName,
            RequireValue(street, nameParameter.Replace("Name", "Street")),
            RequireValue(houseNumber, nameParameter.Replace("Name", "HouseNumber")),
            RequireValue(postalCode, nameParameter.Replace("Name", "PostalCode")),
            RequireValue(city, nameParameter.Replace("Name", "City")),
            country);
    }

    private static bool HasContactValues(params string?[] values) {
        foreach (var value in values) {
            if (!string.IsNullOrWhiteSpace(value)) {
                return true;
            }
        }

        return false;
    }

    private static string RequireValue(string? value, string parameterName) {
        if (!string.IsNullOrWhiteSpace(value)) {
            return value!;
        }

        throw new PSArgumentException($"{parameterName} is required for the selected Swiss QR address shape.", parameterName);
    }
}
