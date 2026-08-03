---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartLine
## SYNOPSIS
Creates line chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartLine [-Name] <string> [-Value] <double[]> [-Color <ChartColor>] [-Marker <ChartMarkerShape>] [-Smooth] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageChart to define a line-series dataset.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChartLine -Name 'Sales' -Value 10,20,18,24 -Color Green -Smooth
```

Creates a smoothed line-series definition ready to be rendered by New-ImageChart.

## PARAMETERS

### -Color
Line color.

```yaml
Type: ChartColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Marker
None suppresses line markers; Circle enables ChartForgeX's shared line-marker style.

```yaml
Type: ChartMarkerShape
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Circle

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Label for the line.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Label
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Smooth
Render the line using a smooth curve.

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

### -Value
Y values for the line.

```yaml
Type: Double[]
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

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None
