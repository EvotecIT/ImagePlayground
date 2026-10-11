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
New-ImageBarCode [-Type] <SymbolFormat> [-Value] <string> [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
Creates a barcode image.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png
```


## PARAMETERS

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

### -Type
QR formats use New-ImageQRCode. MaxiCode requires specialized rendering, and GS1 Composite requires separate payloads; neither is supported by this single-value command.

```yaml
Type: SymbolFormat
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: QrCode, MicroQrCode, Aztec, Code128, Gs1Code128, Code39, Code93, Ean, UpcA, UpcE, Itf14, Itf, Industrial2Of5, Matrix2Of5, Iata2Of5, PatchCode, Codabar, Msi, Code11, Plessey, Telepen, Pharmacode, PharmacodeTwoTrack, Code32, Postnet, Planet, RoyalMail4State, AustraliaPost, JapanPost, Gs1DataBarTruncated, Gs1DataBarOmnidirectional, Gs1DataBarStacked, Gs1DataBarExpanded, Gs1DataBarExpandedStacked, UspsIntelligentMail, KixCode, DataMatrix, Pdf417, MicroPdf417, RmQrCode, Gs1DataBarLimited, Gs1DataBarStackedOmnidirectional, MaxiCode, DotCode, HanXin, Gs1Composite

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
