---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartBoxPlot
## SYNOPSIS
Creates box-plot chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartBoxPlot [-Name] <string> [-X] <double[]> -Minimum <double[]> -Q1 <double[]> -Median <double[]> -Q3 <double[]> -Maximum <double[]> [-Color <ChartColor>] [<CommonParameters>]
```

## DESCRIPTION
Creates box-plot chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartBoxPlot -Name 'Latency' -X 1 -Minimum 2 -Q1 4 -Median 6 -Q3 8 -Maximum 10
```


## PARAMETERS

### -Color
Box plot color.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Maximum
Maximum whisker values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Median
Median values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Minimum
Minimum whisker values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Series label.

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

### -Q1
First quartile values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Q3
Third quartile values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X or category values.

```yaml
Type: Double[]
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

- `None`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None
