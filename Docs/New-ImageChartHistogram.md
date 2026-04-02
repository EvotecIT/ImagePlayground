---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartHistogram
## SYNOPSIS
Creates histogram chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartHistogram [-Name] <string> [-Values] <double[]> [-BinSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Creates histogram chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartHistogram -Name 'Data' -Values @(1,2,3) -BinSize 2
```

## PARAMETERS

### -BinSize
Optional bin size.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Label for the histogram.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Label
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Values
Data values.

```yaml
Type: Double[]
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

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None

