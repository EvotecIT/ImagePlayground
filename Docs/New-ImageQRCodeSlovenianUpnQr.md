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
New-ImageQRCodeSlovenianUpnQr [-Payload] <SlovenianUpnQrPayload> [-FilePath] <string> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet when a prepared Slovenian UPN payment payload should be rendered into a scannable QR image.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $upn = [CodeGlyphX.Payloads.SlovenianUpnQrPayload]::new('John Doe','Main Street 1','Ljubljana','Evotec d.o.o.','Business Street 2','Maribor','SI56192001234567890','Invoice 2026-041',19999)
            New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath upn.png
```

Generates a UPN payment QR code from a complete Slovenian payment payload object.

### EXAMPLE 2
```powershell
PS> $upn = [CodeGlyphX.Payloads.SlovenianUpnQrPayload]::new('John Doe','Main Street 1','Ljubljana','Evotec d.o.o.','Business Street 2','Maribor','SI56192001234567890','Annual subscription',4900)
            New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath upn-brand.png -ForegroundColor DarkGreen -PixelSize 14 -Show
```

Creates a styled payment QR code and opens the resulting image after generation.

## PARAMETERS

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
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

### -Payload
UPN payment payload.

```yaml
Type: SlovenianUpnQrPayload
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

