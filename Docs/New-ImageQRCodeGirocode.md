---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeGirocode
## SYNOPSIS
Generates a Girocode QR code.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeGirocode [-Iban] <string> [-Bic] <string> [-Name] <string> [-Amount] <decimal> [[-RemittanceInformation] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to create SEPA payment QR codes for European bank transfers.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Evotec GmbH' -Amount 12.34 -FilePath giro.png
```

Creates a payment QR code with the core SEPA transfer fields.

### EXAMPLE 2
```powershell
New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Evotec GmbH' -Amount 249.99 -RemittanceInformation 'Invoice 2026-041' -FilePath invoice-payment.png -ForegroundColor DarkBlue -PixelSize 14 -Show
```

Generates a branded invoice-payment QR code and opens it after creation.

## PARAMETERS

### -Amount
Transfer amount.

```yaml
Type: Decimal
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: SixLabors.ImageSharp.Color
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: FFFFFFFF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bic
BIC of the payee.

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

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 5
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: SixLabors.ImageSharp.Color
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Iban
IBAN of the payee.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Recipient name.

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

### -PixelSize
Pixel size for each QR module.

```yaml
Type: System.Int32
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -RemittanceInformation
Optional remittance information.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 4
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

