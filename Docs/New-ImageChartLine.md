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
New-ImageChartLine [-Name] <string> [-Value] <double[]> [-Color <Color>] [-Marker <MarkerShape>] [-Smooth] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageChart to define a line-series dataset.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartLine -Name 'Sales' -Value 10,20,18,24 -Color Green -Smooth
```

Creates a smoothed line-series definition ready to be rendered by New-ImageChart.

## PARAMETERS

### -Color
Line color.

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

### -Marker
Shape of markers placed on data points.

Possible values: None, FilledCircle, OpenCircle, FilledSquare, OpenSquare, FilledTriangleUp, OpenTriangleUp, FilledTriangleDown, OpenTriangleDown, FilledDiamond, OpenDiamond, Eks, Cross, VerticalBar, HorizontalBar, TriUp, TriDown, Asterisk, HashTag, OpenCircleWithDot, OpenCircleWithCross, OpenCircleWithEks, CircleWithLineLeft, CircleWithLineRight, TriangleWithLineLeft, TriangleWithLineRight

```yaml
Type: MarkerShape
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: None, FilledCircle, OpenCircle, FilledSquare, OpenSquare, FilledTriangleUp, OpenTriangleUp, FilledTriangleDown, OpenTriangleDown, FilledDiamond, OpenDiamond, Eks, Cross, VerticalBar, HorizontalBar, TriUp, TriDown, Asterisk, HashTag, OpenCircleWithDot, OpenCircleWithCross, OpenCircleWithEks, CircleWithLineLeft, CircleWithLineRight, TriangleWithLineLeft, TriangleWithLineRight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
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
Accept wildcard characters: True
```

### -Smooth
Render the line using a smooth curve.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Value
Y values for the line.

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

