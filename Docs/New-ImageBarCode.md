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
New-ImageBarCode [-Type] <BarcodeType> [-Value] <string> [-FilePath] <string> [-Async] [<CommonParameters>]
```

## DESCRIPTION
Creates a barcode image.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png
```


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

### -FilePath
Output path for the barcode image.

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

### -Type
Barcode type.

Possible values: Code128, Code93, Code39, KixCode, UPCE, UPCA, EAN, DataMatrix, PDF417

```yaml
Type: BarcodeType
Parameter Sets: __AllParameterSets
Aliases:
Possible values: Code128, GS1_128, Code39, Code93, EAN, UPCA, UPCE, ITF14, ITF, Industrial2of5, Matrix2of5, IATA2of5, PatchCode, Codabar, MSI, Code11, Plessey, Telepen, Pharmacode, PharmacodeTwoTrack, Code32, Postnet, Planet, RoyalMail4State, AustraliaPost, JapanPost, GS1DataBarTruncated, GS1DataBarOmni, GS1DataBarStacked, GS1DataBarExpanded, GS1DataBarExpandedStacked, UspsImb, KixCode, DataMatrix, PDF417, MicroPDF417

Required: True
Position: 0
Default value: Code128
Accept pipeline input: False
Accept wildcard characters: True
```

### -Value
Value encoded in the barcode.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
