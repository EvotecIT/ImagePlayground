---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeBezahlCode
## SYNOPSIS
Generates a BezahlCode QR for German banking payloads.

## SYNTAX
### ContactAccount
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> [-Reason <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### ContactSepa
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> [-Reason <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### NonSepaPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-ExecutionDate <datetime>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### SepaPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> [-Reason <string>] [-Currency <string>] [-ExecutionDate <datetime>] [-SepaReference <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### NonSepaDirectDebit
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> -CreditorId <string> -MandateId <string> -DateOfSignature <datetime> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-ExecutionDate <datetime>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### SepaDirectDebit
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> -CreditorId <string> -MandateId <string> -DateOfSignature <datetime> [-Reason <string>] [-Currency <string>] [-ExecutionDate <datetime>] [-SepaReference <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### NonSepaPeriodicPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> -PeriodicUnitRotation <int> -PeriodicFirstExecutionDate <datetime> -PeriodicLastExecutionDate <datetime> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-PeriodicUnit <QrBezahlPeriodicUnit>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

### SepaPeriodicPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> -PeriodicUnitRotation <int> -PeriodicFirstExecutionDate <datetime> -PeriodicLastExecutionDate <datetime> [-Reason <string>] [-Currency <string>] [-SepaReference <string>] [-PeriodicUnit <QrBezahlPeriodicUnit>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render contact and payment-oriented BezahlCode payloads for German banking scenarios.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeBezahlCode -Authority Contact -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Reason 'Invoice 2026-041' -FilePath bezahl.png
```

Creates a contact-style BezahlCode QR code that can be scanned by BezahlCode-aware banking apps.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeBezahlCode -Authority SinglePaymentSepa -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 249.99 -Reason 'Consulting Retainer' -ExecutionDate (Get-Date).Date.AddDays(3) -FilePath bezahl-sepa.png -ForegroundColor Navy -BackgroundColor WhiteSmoke -PixelSize 16 -Show
```

Produces a payment-oriented BezahlCode QR code with custom styling and opens it after generation.

## PARAMETERS

### -Account
Account number for non-SEPA authorities.

```yaml
Type: String
Parameter Sets: ContactAccount, NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Amount
Payment amount for payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Authority
Payment authority type.

```yaml
Type: QrBezahlAuthorityType
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values: SinglePayment, SinglePaymentSepa, SingleDirectDebit, SingleDirectDebitSepa, PeriodicSinglePayment, PeriodicSinglePaymentSepa, Contact, ContactV2

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: Color
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bic
BIC/SWIFT code for SEPA authorities.

```yaml
Type: String
Parameter Sets: ContactSepa, SepaPayment, SepaDirectDebit, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bnc
Bank number code for non-SEPA authorities.

```yaml
Type: String
Parameter Sets: ContactAccount, NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -CreditorId
Creditor identifier for direct-debit authorities.

```yaml
Type: String
Parameter Sets: NonSepaDirectDebit, SepaDirectDebit
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Currency
Currency code for payment authorities.

```yaml
Type: String
Parameter Sets: NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -DateOfSignature
Mandate signature date for direct-debit authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaDirectDebit, SepaDirectDebit
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ExecutionDate
Execution date for single-payment and direct-debit authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 4
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Iban
International bank account number for SEPA authorities.

```yaml
Type: String
Parameter Sets: ContactSepa, SepaPayment, SepaDirectDebit, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -MandateId
Mandate identifier for direct-debit authorities.

```yaml
Type: String
Parameter Sets: NonSepaDirectDebit, SepaDirectDebit
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Payer or payee name.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PeriodicFirstExecutionDate
First execution date for periodic-payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PeriodicLastExecutionDate
Last execution date for periodic-payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PeriodicUnit
Periodic unit for periodic-payment authorities.

```yaml
Type: QrBezahlPeriodicUnit
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values: Weekly, Monthly

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PeriodicUnitRotation
Periodic unit rotation for periodic-payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PostingKey
Posting key for non-SEPA payment authorities.

```yaml
Type: String
Parameter Sets: NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Reason
Reason for payment.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -SepaReference
SEPA reference for SEPA payment authorities.

```yaml
Type: String
Parameter Sets: SepaPayment, SepaDirectDebit, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Opens the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None
