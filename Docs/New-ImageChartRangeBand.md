---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartRangeBand
## SYNOPSIS
Creates range-band chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartRangeBand [-Name] <string> [-X] <double[]> [-Lower] <double[]> [-Upper] <double[]> [-Color <ChartColor>] [-Area] [-NoSmooth] [<CommonParameters>]
```

## DESCRIPTION
Creates range-band chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartRangeBand -Name 'Expected' -X 1,2 -Lower 8,9 -Upper 12,15 -Area
```


## PARAMETERS

### -Area
Render the range as an area.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Color
Range band color.

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

### -Lower
Lower values.

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

### -NoSmooth
Render range-area boundaries without smoothing.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Upper
Upper values.

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
