---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageBarCode
## SYNOPSIS
Reads barcode information from an image file.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageBarCode [-FilePath] <string> [-Type <SymbolFormat[]>] [-TimeoutSeconds <double>] [-ScanOptions <ScanOptions>] [-Detailed] [<CommonParameters>]
```

## DESCRIPTION
Reads barcode information from an image file.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageBarCode -FilePath barcode.png
```


### EXAMPLE 2
```powershell
PS> Get-ChildItem *.png | Get-ImageBarCode -Type Ean, Code128 -TimeoutSeconds 2 | Select-Object Type, Text
```

Restricting the formats makes scans faster and avoids false positives from unrelated symbologies.

### EXAMPLE 3
```powershell
PS> $scan = Get-ImageBarCode -FilePath barcode.png -Type DataMatrix -Detailed
$scan.CompletionReason
$scan.Symbols
```


## PARAMETERS

### -Detailed
Without this switch, return the first DetectedSymbol; cancellation and deadlines produce errors. Detailed results retain structured cancellation and deadline status.

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

### -FilePath
The file must exist.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: FullName, Path
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue, ByPropertyName)
Accept wildcard characters: False
```

### -ScanOptions
When omitted, scans allow five seconds and use CodeGlyphX default scan formats, excluding QR formats. Other catalogue formats can be selected through Type or ScanOptions.Formats when ScanOptions is supplied. QrCode, MicroQrCode, and RmQrCode are rejected through either selection path; use Get-ImageQRCode, QrCode.Read, or SymbolScanner for QR recognition. A new ScanOptions object uses CodeGlyphX defaults, including its 500 ms total deadline.

```yaml
Type: ScanOptions
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TimeoutSeconds
Scan deadline in seconds. Defaults to five seconds.

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

### -Type
Barcode formats to look for. Defaults to CodeGlyphX default scan formats, excluding QR formats. Select other catalogue formats through Type or ScanOptions.Formats. QrCode, MicroQrCode, and RmQrCode are rejected; use Get-ImageQRCode, QrCode.Read, or SymbolScanner for QR recognition.

```yaml
Type: SymbolFormat[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: QrCode, MicroQrCode, Aztec, Code128, Gs1Code128, Code39, Code93, Ean, UpcA, UpcE, Itf14, Itf, Industrial2Of5, Matrix2Of5, Iata2Of5, PatchCode, Codabar, Msi, Code11, Plessey, Telepen, Pharmacode, PharmacodeTwoTrack, Code32, Postnet, Planet, RoyalMail4State, AustraliaPost, JapanPost, Gs1DataBarTruncated, Gs1DataBarOmnidirectional, Gs1DataBarStacked, Gs1DataBarExpanded, Gs1DataBarExpandedStacked, UspsIntelligentMail, KixCode, DataMatrix, Pdf417, MicroPdf417, RmQrCode, Gs1DataBarLimited, Gs1DataBarStackedOmnidirectional, MaxiCode, DotCode, HanXin, Gs1Composite, Ean2, Ean5, TelepenNumeric, Coop2Of5, Datalogic2Of5, Bc412, FacingIdentificationMark, Daft, Flattermarken, ChannelCode, Code16K, Code49, CodablockF, PosiCode, Mailmark4State, D3Aqr, AztecRune, CodeOne, Ultracode

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

- `CodeGlyphX.DetectedSymbol`
- `CodeGlyphX.ScanResult`

## RELATED LINKS

- None
