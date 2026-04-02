---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeSkypeCall
## SYNOPSIS
Generates a QR code initiating a Skype call.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeSkypeCall [-UserName] <string> [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet for legacy Skype calling scenarios where a QR scan should start a call with a specific account.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath skype.png
```

Creates a QR code that opens Skype and targets the selected username for a call.

### EXAMPLE 2
```powershell
New-ImageQRCodeSkypeCall -UserName 'evotec.helpdesk' -FilePath skype-helpdesk.png -ForegroundColor MidnightBlue -PixelSize 16 -Show
```

Generates a branded Skype call QR code and opens the resulting image after creation.

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

### -UserName
Skype username to call.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

