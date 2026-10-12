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
Get-ImageBarCode [-FilePath] <string> [-ScanOptions <ScanOptions>] [-Detailed] [<CommonParameters>]
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
PS> $options = [CodeGlyphX.ScanOptions]::new()
$options.Formats = [CodeGlyphX.SymbolFormat[]]@('Ean', 'DataMatrix')
$options.TimeoutMilliseconds = 5000
$scan = Get-ImageBarCode -FilePath barcode.png -ScanOptions $options -Detailed
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
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ScanOptions
When omitted, scans allow five seconds and exclude QR, Pharmacode, and Patch Code. Explicit formats can opt into Pharmacode or Patch Code. A new ScanOptions object uses CodeGlyphX defaults, including its 500 ms total deadline.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `CodeGlyphX.DetectedSymbol`
- `CodeGlyphX.ScanResult`

## RELATED LINKS

- None
