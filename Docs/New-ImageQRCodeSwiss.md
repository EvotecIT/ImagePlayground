---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeSwiss
## SYNOPSIS
Generates a Swiss QR payment code.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeSwiss [-Payload] <PayloadGenerator+SwissQrCode> [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet when a prepared SwissQrCodePayload should be rendered into a payment QR image.

## EXAMPLES

### EXAMPLE 1
```powershell
$swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
            New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss.png
```

Renders a Swiss payment QR code from a previously prepared payment payload object.

### EXAMPLE 2
```powershell
$swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
            New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show
```

Creates a branded QR image and opens it immediately after generation.

## PARAMETERS

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

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
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

### -Payload
Swiss QR payload data.

```yaml
Type: SwissQrCode
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
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

### -Show
Opens the image once generated.

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

