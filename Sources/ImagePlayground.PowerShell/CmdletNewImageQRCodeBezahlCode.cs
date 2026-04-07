using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a BezahlCode QR for German banking payloads.</summary>
/// <para>Use this cmdlet to render contact and payment-oriented BezahlCode payloads for German banking scenarios.</para>
/// <example>
///   <summary>Create a basic BezahlCode contact QR</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeBezahlCode -Authority Contact -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Reason 'Invoice 2026-041' -FilePath bezahl.png</code>
///   <para>Creates a contact-style BezahlCode QR code that can be scanned by BezahlCode-aware banking apps.</para>
/// </example>
/// <example>
///   <summary>Create a SEPA single-payment BezahlCode image</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeBezahlCode -Authority SinglePaymentSepa -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 249.99 -Reason 'Consulting Retainer' -ExecutionDate (Get-Date).Date.AddDays(3) -FilePath bezahl-sepa.png -ForegroundColor Navy -BackgroundColor WhiteSmoke -PixelSize 16 -Show</code>
///   <para>Produces a payment-oriented BezahlCode QR code with custom styling and opens it after generation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeBezahlCode")]
public sealed class NewImageQrCodeBezahlCodeCmdlet : AsyncQrCodeCmdlet {
    private const string ParameterSetContactAccount = "ContactAccount";
    private const string ParameterSetContactSepa = "ContactSepa";
    private const string ParameterSetNonSepaPayment = "NonSepaPayment";
    private const string ParameterSetSepaPayment = "SepaPayment";
    private const string ParameterSetNonSepaDirectDebit = "NonSepaDirectDebit";
    private const string ParameterSetSepaDirectDebit = "SepaDirectDebit";
    private const string ParameterSetNonSepaPeriodicPayment = "NonSepaPeriodicPayment";
    private const string ParameterSetSepaPeriodicPayment = "SepaPeriodicPayment";

    /// <summary>Payment authority type.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetContactAccount)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetContactSepa)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public QrBezahlAuthorityType Authority { get; set; }

    /// <summary>Payer or payee name.</summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetContactAccount)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetContactSepa)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Account number for non-SEPA authorities.</summary>
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetContactAccount)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    public string Account { get; set; } = string.Empty;

    /// <summary>Bank number code for non-SEPA authorities.</summary>
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetContactAccount)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    public string Bnc { get; set; } = string.Empty;

    /// <summary>International bank account number for SEPA authorities.</summary>
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetContactSepa)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string Iban { get; set; } = string.Empty;

    /// <summary>BIC/SWIFT code for SEPA authorities.</summary>
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetContactSepa)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(Mandatory = true, Position = 3, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string Bic { get; set; } = string.Empty;

    /// <summary>Reason for payment.</summary>
    [Parameter(ParameterSetName = ParameterSetContactAccount)]
    [Parameter(ParameterSetName = ParameterSetContactSepa)]
    [Parameter(ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>Payment amount for payment authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public decimal? Amount { get; set; }

    /// <summary>Currency code for payment authorities.</summary>
    [Parameter(ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string Currency { get; set; } = "EUR";

    /// <summary>Posting key for non-SEPA payment authorities.</summary>
    [Parameter(ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    public string? PostingKey { get; set; }

    /// <summary>Execution date for single-payment and direct-debit authorities.</summary>
    [Parameter(ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetSepaDirectDebit)]
    public DateTime? ExecutionDate { get; set; }

    /// <summary>SEPA reference for SEPA payment authorities.</summary>
    [Parameter(ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string? SepaReference { get; set; }

    /// <summary>Creditor identifier for direct-debit authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaDirectDebit)]
    public string? CreditorId { get; set; }

    /// <summary>Mandate identifier for direct-debit authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaDirectDebit)]
    public string? MandateId { get; set; }

    /// <summary>Mandate signature date for direct-debit authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaDirectDebit)]
    public DateTime? DateOfSignature { get; set; }

    /// <summary>Periodic unit for periodic-payment authorities.</summary>
    [Parameter(ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public QrBezahlPeriodicUnit PeriodicUnit { get; set; } = QrBezahlPeriodicUnit.Monthly;

    /// <summary>Periodic unit rotation for periodic-payment authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public int? PeriodicUnitRotation { get; set; }

    /// <summary>First execution date for periodic-payment authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public DateTime? PeriodicFirstExecutionDate { get; set; }

    /// <summary>Last execution date for periodic-payment authorities.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public DateTime? PeriodicLastExecutionDate { get; set; }

    /// <summary>Output image path.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetContactAccount)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetContactSepa)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetNonSepaPayment)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetSepaPayment)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetNonSepaDirectDebit)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetSepaDirectDebit)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetNonSepaPeriodicPayment)]
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4, ParameterSetName = ParameterSetSepaPeriodicPayment)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image after creation.</summary>
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
        ValidateAuthorityParameters();
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateBezahlCodeAsync(Authority, Name, Account, Bnc, Iban, Bic, Reason, FilePath, Amount, Currency, PostingKey, ExecutionDate, SepaReference, CreditorId, MandateId, DateOfSignature, PeriodicUnit, PeriodicUnitRotation, PeriodicFirstExecutionDate, PeriodicLastExecutionDate, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateBezahlCode(Authority, Name, Account, Bnc, Iban, Bic, Reason, FilePath, Amount, Currency, PostingKey, ExecutionDate, SepaReference, CreditorId, MandateId, DateOfSignature, PeriodicUnit, PeriodicUnitRotation, PeriodicFirstExecutionDate, PeriodicLastExecutionDate, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }

    private void ValidateAuthorityParameters() {
        switch (ParameterSetName) {
            case ParameterSetContactAccount:
                ValidateAuthorityParameterSet(ParameterSetContactAccount, QrBezahlAuthorityType.Contact, QrBezahlAuthorityType.ContactV2);
                break;
            case ParameterSetContactSepa:
                ValidateAuthorityParameterSet(ParameterSetContactSepa, QrBezahlAuthorityType.ContactV2);
                break;
            case ParameterSetNonSepaPayment:
                ValidateAuthorityParameterSet(ParameterSetNonSepaPayment, QrBezahlAuthorityType.SinglePayment);
                break;
            case ParameterSetSepaPayment:
                ValidateAuthorityParameterSet(ParameterSetSepaPayment, QrBezahlAuthorityType.SinglePaymentSepa);
                break;
            case ParameterSetNonSepaDirectDebit:
                ValidateAuthorityParameterSet(ParameterSetNonSepaDirectDebit, QrBezahlAuthorityType.SingleDirectDebit);
                break;
            case ParameterSetSepaDirectDebit:
                ValidateAuthorityParameterSet(ParameterSetSepaDirectDebit, QrBezahlAuthorityType.SingleDirectDebitSepa);
                break;
            case ParameterSetNonSepaPeriodicPayment:
                ValidateAuthorityParameterSet(ParameterSetNonSepaPeriodicPayment, QrBezahlAuthorityType.PeriodicSinglePayment);
                break;
            case ParameterSetSepaPeriodicPayment:
                ValidateAuthorityParameterSet(ParameterSetSepaPeriodicPayment, QrBezahlAuthorityType.PeriodicSinglePaymentSepa);
                break;
        }
    }

    private void ValidateAuthorityParameterSet(string parameterSetName, params QrBezahlAuthorityType[] validAuthorities) {
        foreach (QrBezahlAuthorityType validAuthority in validAuthorities) {
            if (Authority == validAuthority) {
                return;
            }
        }

        throw new PSArgumentException($"Authority {Authority} is not valid for parameter set {parameterSetName}.", nameof(Authority));
    }
}
