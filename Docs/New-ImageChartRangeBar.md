---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartRangeBar
## SYNOPSIS
Creates range-bar chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartRangeBar [-Name] <string> [-X] <double[]> [-Start] <double[]> [-End] <double[]> [-Color <ChartColor>] [<CommonParameters>]
```

## DESCRIPTION
Creates range-bar chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartRangeBar -Name 'Maintenance' -X 1,2 -Start 2,4 -End 5,8 -Color Orange
```


## PARAMETERS

### -Color
Range bar color.

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

### -End
Interval end values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
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

### -Start
Interval start values.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
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
