---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageBarCode
## SYNOPSIS
Creates a barcode image.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageBarCode [-Type] <SymbolFormat> [-Value] <string> [-FilePath] <string> [-ModuleSize <int>] [-QuietZone <int>] [-Height <int>] [-ShowText] [-Text <string>] [-ForegroundColor <OfficeColor>] [-BackgroundColor <OfficeColor>] [<CommonParameters>]
```

## DESCRIPTION
The file extension picks the format (.png, .svg, .jpg, .webp and others). PNG output is compressed.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png
```


### EXAMPLE 2
```powershell
PS> New-ImageBarCode -Type Code128 -Value 'EVO-LAP-0142' -FilePath tag.png -ShowText -ModuleSize 3 -Height 30
```

-ShowText prints the encoded value under linear symbols; -Text prints custom text instead.

### EXAMPLE 3
```powershell
PS> New-ImageBarCode -Type DataMatrix -Value 'LOT-2026-0042' -FilePath lot.png -ModuleSize 8 -ForegroundColor '#1e1b4b'
```


## PARAMETERS

### -BackgroundColor
Background colour.

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

### -FilePath
Output path for the barcode image.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ForegroundColor
Bar colour.

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

### -Height
Bar height in modules. Linear symbols only.

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

### -ModuleSize
Pixel width of one module: the narrowest bar for linear symbols, one cell for 2D symbols.

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

### -QuietZone
Quiet zone (blank border) in modules.

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

### -ShowText
Print the encoded value under the bars. Linear symbols only.

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

### -Text
Custom human-readable text printed under the bars. Linear symbols only.

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

### -Type
QR formats use New-ImageQRCode. MaxiCode requires specialized rendering, and GS1 Composite requires separate payloads; neither is supported by this single-value command.

```yaml
Type: SymbolFormat
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: QrCode, MicroQrCode, Aztec, Code128, Gs1Code128, Code39, Code93, Ean, UpcA, UpcE, Itf14, Itf, Industrial2Of5, Matrix2Of5, Iata2Of5, PatchCode, Codabar, Msi, Code11, Plessey, Telepen, Pharmacode, PharmacodeTwoTrack, Code32, Postnet, Planet, RoyalMail4State, AustraliaPost, JapanPost, Gs1DataBarTruncated, Gs1DataBarOmnidirectional, Gs1DataBarStacked, Gs1DataBarExpanded, Gs1DataBarExpandedStacked, UspsIntelligentMail, KixCode, DataMatrix, Pdf417, MicroPdf417, RmQrCode, Gs1DataBarLimited, Gs1DataBarStackedOmnidirectional, MaxiCode, DotCode, HanXin, Gs1Composite, Ean2, Ean5, TelepenNumeric, Coop2Of5, Datalogic2Of5, Bc412, FacingIdentificationMark, Daft, Flattermarken, ChannelCode, Code16K, Code49, CodablockF, PosiCode, Mailmark4State, D3Aqr, AztecRune, CodeOne, Ultracode

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Value encoded in the barcode.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
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
