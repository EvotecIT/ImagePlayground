---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeWiFi
## SYNOPSIS
Creates a WiFi QR code image.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeWiFi [-SSID] <string> [-Password] <string> [-FilePath] <string> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
The generated QR code uses a WiFi payload that can be scanned by mobile devices to prefill network settings.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeWiFi -SSID Test -Password pass123 -FilePath wifi.png
```

## PARAMETERS

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
Position: 2
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

### -Password
WiFi password.

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

### -Show
Open the image after creation.

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

### -SSID
WiFi network name.

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

