---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCode
## SYNOPSIS
Generates a QR code image from plain text content.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCode [-Content] <string> [-FilePath] <string> [-Show] [-LogoPath <string>] [-Preset <QrHeroPreset>] [-EyeBall <QrModuleShape>] [-Gradient <OfficeColor[]>] [-Frame <QrCanvasTemplate>] [-Caption <string>] [-Transparent] [-ForegroundColor <OfficeColor>] [-BackgroundColor <OfficeColor>] [-PixelSize <int>] [-Size <int>] [-QuietZone <int>] [-ErrorCorrection <QrErrorCorrectionLevel>] [-ModuleShape <QrModuleShape>] [-EyeShape <QrModuleShape>] [-EyeColor <OfficeColor>] [-LogoScale <double>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet when the QR payload is already available as a raw string.

Styled artwork and budgeted logos must pass the native CodeGlyphX guardrails. The limited decoder used by Windows PowerShell 5.1 can reject artwork accepted by PowerShell 7. Rendering then throws rather than writing unverified output; simplify the design or validate it with a modern runtime.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCode -Content 'https://evotec.xyz' -FilePath qr.png
```


### EXAMPLE 2
```powershell
PS> New-ImageQRCode -Content 'text' -FilePath qr.png -Show
```


### EXAMPLE 3
```powershell
PS> New-ImageQRCode -Content 'https://evotec.xyz' -FilePath qr-logo.png -LogoPath logo.png
```


### EXAMPLE 4
```powershell
PS> New-ImageQRCode -Content 'https://evotec.xyz' -FilePath styled.png -ModuleShape Rounded -EyeShape Circle -EyeColor '#4c1d95' -ForegroundColor '#1e1b4b' -Size 600 -ErrorCorrection H
```

Rounded data modules, circular eyes in an accent colour, and an image at most 600 px wide. High error correction leaves room for styling and logos.

### EXAMPLE 5
```powershell
PS> 'EVO-LAP-0142', 'EVO-LAP-0177' | ForEach-Object { New-ImageQRCode -Content "https://assets.example.com/$_" -FilePath "$_.png" -QuietZone 2 -Size 300 }
```

PNG output is compressed; a typical URL code at 300 px is about 1 KB.

### EXAMPLE 6
```powershell
PS> New-ImageQRCode -Content 'https://example.com/menu' -FilePath menu.png -Preset Sunset -Frame ScanMePill -Caption 'VIEW THE MENU' -ErrorCorrection H
```


### EXAMPLE 7
```powershell
PS> New-ImageQRCode -Content 'https://example.com' -FilePath styled.svg -Preset Ink -EyeShape Rounded -EyeBall Circle -ErrorCorrection H
```


### EXAMPLE 8
```powershell
PS> New-ImageQRCode -Content 'https://example.com' -FilePath brand.png -Preset Brand -Gradient '#431b1b', '#8c2059' -Logo logo.png -LogoScale 0.3 -ErrorCorrection H
```


## PARAMETERS

### -BackgroundColor
Background color of the QR code.

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Caption
Caption below the code. A frame uses SCAN ME when no caption is supplied.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Content
Content encoded in the QR code.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ErrorCorrection
Higher levels survive more damage, logos, and styling at the cost of a denser code.

```yaml
Type: QrErrorCorrectionLevel
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: L, M, Q, H

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EyeBall
Shape of the finder centres, independently of -EyeShape.

```yaml
Type: QrModuleShape
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Square, Circle, Rounded, ConnectedRounded, ConnectedSquircle, Diamond, SoftDiamond, Squircle, Leaf, Wave, Blob, Dot, DotGrid, Liquid, Classy, ExtraRounded, BarsVertical, BarsHorizontal, Plus, Star, Heart, CutCorner

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EyeColor
Colour of the three finder patterns (eyes).

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EyeShape
Shape of the three finder patterns (eyes), for example Square, Rounded, or Circle.

```yaml
Type: QrModuleShape
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Square, Circle, Rounded, ConnectedRounded, ConnectedSquircle, Diamond, SoftDiamond, Squircle, Leaf, Wave, Blob, Dot, DotGrid, Liquid, Classy, ExtraRounded, BarsVertical, BarsHorizontal, Plus, Star, Heart, CutCorner

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
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Frame
Frame template outside the complete QR quiet zone.

```yaml
Type: QrCanvasTemplate
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, ScanMePill, SpeechBubble, TicketStub, PhoneOutline, RoundedCardHeader, CornerViewfinder

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Gradient
Two foreground colours, from left to right, for example '#9c3411', '#8c2059'.

```yaml
Type: OfficeColor[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LogoPath
CodeGlyphX sizes the padded logo against each affected correction block. By default, the correction-block budget determines the largest safe placement; -LogoScale adds a caller-selected size limit. Unreadable or invisible artwork fails before replacing the output. Raster and SVG formats use the same placement.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Logo
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LogoScale
Logo width as a fraction of the symbol width (0.05 to 0.4). Omit this option to use the correction-block budget. Use -ErrorCorrection H for larger logos.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ModuleShape
Finder and timing patterns stay readable. SVG supports the coordinated shapes through vector coverage contours.

```yaml
Type: QrModuleShape
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Square, Circle, Rounded, ConnectedRounded, ConnectedSquircle, Diamond, SoftDiamond, Squircle, Leaf, Wave, Blob, Dot, DotGrid, Liquid, Classy, ExtraRounded, BarsVertical, BarsHorizontal, Plus, Star, Heart, CutCorner

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PixelSize
Pixel size for each QR module. Ignored when -Size is supplied.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Preset
Coordinated QR appearance, such as Ink, Sunset, Ocean or Brand.

```yaml
Type: QrHeroPreset
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Ink, DotMatrix, Sunset, Ocean, MonoClassy, Brand, Neon, PaperCutout, RetroPixel, MinimalLine, Botanical, Berry

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -QuietZone
Quiet zone (blank border) in modules. Defaults to 4, the size scanners expect.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Size
The QR panel includes its quiet zone and uses the largest whole number of module pixels that fits, so the panel can be a few pixels smaller than requested. Frames, captions, padding, and shadows add pixels outside the panel.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Transparent
Create the QR code with a transparent background.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
