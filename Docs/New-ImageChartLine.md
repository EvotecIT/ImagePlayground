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
New-ImageChartLine [-Name] <string> [-Value] <double[]> [-Color <ChartColor>] [-Marker <ChartMarkerShape>] [-MarkerSize <double>] [-Smooth] [-SecondaryAxis] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageChart to define a line-series dataset. Lines can share a chart with bar, area, scatter, and other line definitions.

Markers are configured per series, so one line can show markers while another does not.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChartLine -Name 'Sales' -Value 10,20,18,24 -Color Green -Smooth
```

Creates a smoothed line-series definition ready to be rendered by New-ImageChart.

### EXAMPLE 2
```powershell
PS> New-ImageChart -XLabel Mon, Tue, Wed, Thu -SecondaryYTitle 'Error rate (%)' -FilePath combo.png {
    New-ImageChartBar -Name Requests -Value 410, 380, 455, 520
    New-ImageChartLine -Name 'Error rate' -Value 1.2, 0.8, 2.4, 1.1 -Marker Diamond -MarkerSize 8 -SecondaryAxis
}
```

Combines bars and a marked line; the line uses the right-hand axis.

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
None (the default) draws no markers. Circle, Square, Diamond, Triangle, Plus, Cross, and Star draw markers on this series only.

```yaml
Type: ChartMarkerShape
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Circle, Square, Diamond, Triangle, Plus, Cross, Star

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MarkerSize
Marker diameter in pixels. Supplying it without -Marker draws circles.

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

### -SecondaryAxis
Plot the line against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.

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

- `ImagePlayground.ChartLine`: Line chart definition.

## RELATED LINKS

- None
