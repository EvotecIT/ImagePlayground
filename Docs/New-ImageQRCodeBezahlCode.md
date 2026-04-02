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
### __AllParameterSets
```powershell
New-ImageQRCodeBezahlCode [-Authority] <QrBezahlAuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-Iban] <string> [-Bic] <string> [-Reason] <string> [-FilePath] <string> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render BezahlCode payment payloads for German banking scenarios.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Reason 'Invoice 2026-041' -FilePath bezahl.png
```

Creates a standard payment QR code that can be scanned by BezahlCode-aware banking apps.

### EXAMPLE 2
```powershell
New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Reason 'Consulting Retainer' -FilePath bezahl-brand.png -ForegroundColor Navy -BackgroundColor WhiteSmoke -PixelSize 16 -Show
```

Produces a payment QR code with custom styling and opens it after generation.

## PARAMETERS

### -Account
Account number.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Authority
Payment authority type.

Possible values: SinglePayment, SinglePaymentSepa, SingleDirectDebit, SingleDirectDebitSepa, PeriodicSinglePayment, PeriodicSinglePaymentSepa, Contact, ContactV2

```yaml
Type: QrBezahlAuthorityType
Parameter Sets: __AllParameterSets
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
Parameter Sets: __AllParameterSets
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
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bnc
Bank number code.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 7
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
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
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Payer or payee name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -Reason
Reason for payment.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Opens the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
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

