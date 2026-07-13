---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeSlovenianUpnQr
## SYNOPSIS
Generates a Slovenian UPN QR payment code.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeSlovenianUpnQr [-PayerName] <string> [-PayerAddress] <string> [-PayerPlace] <string> [-RecipientName] <string> [-RecipientAddress] <string> [-RecipientPlace] <string> [-RecipientIban] <string> [-Description] <string> [-Amount] <double> [-FilePath] <string> [-Deadline <datetime>] [-RecipientSiModel <string>] [-RecipientSiReference <string>] [-Code <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render Slovenian UPN payment details into a scannable QR image.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -FilePath upn.png
```

Generates a UPN payment QR code from the payment fields supplied to the cmdlet.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Annual subscription' -Amount 49 -FilePath upn-brand.png -ForegroundColor DarkGreen -PixelSize 14 -Show
```

Creates a styled payment QR code and opens the resulting image after generation.

## PARAMETERS

### -Amount
Payment amount.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 8
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Code
UPN payment code.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Deadline
Payment deadline.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Description
Payment description.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 7
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 9
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PayerAddress
Payer street address.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PayerName
Payer name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PayerPlace
Payer postal place.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientAddress
Recipient street address.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientIban
Recipient IBAN.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientName
Recipient name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientPlace
Recipient postal place.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientSiModel
Recipient SI model.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RecipientSiReference
Recipient SI reference.

```yaml
Type: String
Parameter Sets: __AllParameterSets
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
Parameter Sets: __AllParameterSets
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
