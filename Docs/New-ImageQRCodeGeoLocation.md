---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeGeoLocation
## SYNOPSIS
Generates a QR code with geolocation data.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeGeoLocation [-Latitude] <string> [-Longitude] <string> [-FilePath] <string> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to create location QR codes that open map applications at a specific coordinate.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeGeoLocation -Latitude '52.2297' -Longitude '21.0122' -FilePath geo.png
```

Generates a QR code that opens the target coordinates in compatible map applications.

### EXAMPLE 2
```powershell
New-ImageQRCodeGeoLocation -Latitude '51.1079' -Longitude '17.0385' -FilePath venue.png -ForegroundColor DarkGreen -PixelSize 16 -Show
```

Creates a location QR for signage, invitations, or venue directions and previews it immediately.

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

### -Latitude
Latitude value.

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

### -Longitude
Longitude value.

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
Open image after creation.

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

