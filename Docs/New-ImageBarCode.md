---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageBarCode
## SYNOPSIS
New-ImageBarCode [-Type] <BarcodeType> [-Value] <string> [-FilePath] <string> [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageBarCode [-Type] <BarcodeType> [-Value] <string> [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
New-ImageBarCode [-Type] <BarcodeType> [-Value] <string> [-FilePath] <string> [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageBarCode -FilePath 'C:\Path'
```

## PARAMETERS

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Type
{{ Fill Type Description }}

```yaml
Type: BarcodeType
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Code128, Code93, Code39, KixCode, UPCE, UPCA, EAN, DataMatrix, PDF417

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Value
{{ Fill Value Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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

- `System.Object`

## RELATED LINKS

- None

