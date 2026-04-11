---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeBezahlCode
## SYNOPSIS
Generates a BezahlCode QR for German payments.

## SYNTAX
### ContactAccount
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> [-Reason <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### ContactSepa
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> [-Reason <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### NonSepaPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-ExecutionDate <datetime>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### SepaPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> [-Reason <string>] [-Currency <string>] [-ExecutionDate <datetime>] [-SepaReference <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### NonSepaDirectDebit
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> -CreditorId <string> -MandateId <string> -DateOfSignature <datetime> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-ExecutionDate <datetime>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### SepaDirectDebit
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> -CreditorId <string> -MandateId <string> -DateOfSignature <datetime> [-Reason <string>] [-Currency <string>] [-ExecutionDate <datetime>] [-SepaReference <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### NonSepaPeriodicPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-FilePath] <string> -Amount <decimal> -PeriodicUnitRotation <int> -PeriodicFirstExecutionDate <datetime> -PeriodicLastExecutionDate <datetime> [-Reason <string>] [-Currency <string>] [-PostingKey <string>] [-PeriodicUnit <QrBezahlPeriodicUnit>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

### SepaPeriodicPayment
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Iban] <string> [-Bic] <string> [-FilePath] <string> -Amount <decimal> -PeriodicUnitRotation <int> -PeriodicFirstExecutionDate <datetime> -PeriodicLastExecutionDate <datetime> [-Reason <string>] [-Currency <string>] [-SepaReference <string>] [-PeriodicUnit <QrBezahlPeriodicUnit>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render BezahlCode payment payloads for German banking scenarios.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Reason 'Invoice 2026-041' -FilePath bezahl.png
```

Creates a standard payment QR code that can be scanned by BezahlCode-aware banking apps.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Reason 'Consulting Retainer' -FilePath bezahl-brand.png -ForegroundColor Navy -BackgroundColor WhiteSmoke -PixelSize 16 -Show
```

Produces a payment QR code with custom styling and opens it after generation.

## PARAMETERS

### -Account
Account number.

```yaml
Type: String
Parameter Sets: ContactAccount, NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Authority
Payment authority type.

Possible values: SinglePayment, SinglePaymentSepa, SingleDirectDebit, SingleDirectDebitSepa, PeriodicSinglePayment, PeriodicSinglePaymentSepa, Contact, ContactV2

```yaml
Type: QrBezahlAuthorityType
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: SinglePayment, SinglePaymentSepa, SingleDirectDebit, SingleDirectDebitSepa, PeriodicSinglePayment, PeriodicSinglePaymentSepa, Contact, ContactV2

Required: True
Position: 0
Default value: SinglePayment
Accept pipeline input: False
Accept wildcard characters: True
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: Color
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: FFFFFFFF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bic
BIC/SWIFT code.

```yaml
Type: String
Parameter Sets: ContactSepa, SepaPayment, SepaDirectDebit, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bnc
Bank number code.

```yaml
Type: String
Parameter Sets: ContactAccount, NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Currency
Currency code for payment authorities.

```yaml
Type: String
Parameter Sets: NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DateOfSignature
Mandate signature date for direct-debit authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaDirectDebit, SepaDirectDebit
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExecutionDate
Execution date for single-payment and direct-debit authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Iban
International bank account number.

```yaml
Type: String
Parameter Sets: ContactSepa, SepaPayment, SepaDirectDebit, SepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Payer or payee name.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PeriodicLastExecutionDate
Last execution date for periodic-payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PeriodicUnit
Periodic unit for periodic-payment authorities.

```yaml
Type: QrBezahlPeriodicUnit
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: Weekly, Monthly

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PeriodicUnitRotation
Periodic unit rotation for periodic-payment authorities.

```yaml
Type: Nullable`1
Parameter Sets: NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -PostingKey
Posting key for non-SEPA payment authorities.

```yaml
Type: String
Parameter Sets: NonSepaPayment, NonSepaDirectDebit, NonSepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Reason
Reason for payment.

```yaml
Type: String
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
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
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Opens the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ContactAccount, ContactSepa, NonSepaPayment, SepaPayment, NonSepaDirectDebit, SepaDirectDebit, NonSepaPeriodicPayment, SepaPeriodicPayment
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

