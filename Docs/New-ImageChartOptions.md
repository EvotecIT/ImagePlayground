---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartOptions
## SYNOPSIS
Creates renderer options for New-ImageChart.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartOptions [-Palette <ChartColor[]>] [-ShowLegend] [-NoLegend] [-ShowPointLegend] [-LegendPosition <ChartLegendPosition>] [-NoAxes] [-NoCard] [-NoPlotBackground] [-Transparent] [-ShowDataLabels] [-TickCount <int>] [-HeatmapScale <ChartHeatmapColorScale>] [-NoHeatmapScale] [-PieLabelContent <ChartPieLabelContent>] [-DonutInnerRadiusRatio <double>] [-DonutCenterValue <string>] [-DonutCenterLabel <string>] [-ProgressMaximum <double>] [-NoProgressHandles] [-PictorialSymbol <ChartPictorialSymbol>] [-PictorialColumns <int>] [-WordCloudMaximumTerms <int>] [<CommonParameters>]
```

## DESCRIPTION
Creates renderer options for New-ImageChart.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartOptions
```

## PARAMETERS

### -DonutCenterLabel
Donut center label text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -DonutCenterValue
Donut center value text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -DonutInnerRadiusRatio
Donut inner radius ratio.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -HeatmapScale
Heatmap color scale.

```yaml
Type: ChartHeatmapColorScale
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Sequential, Semantic

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -LegendPosition
Legend placement.

```yaml
Type: ChartLegendPosition
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Bottom, Top, Left, Right

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoAxes
Hide axes.

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

### -NoCard
Hide chart card surface.

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

### -NoHeatmapScale
Hide heatmap scale legend.

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

### -NoLegend
Hide the legend.

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

### -NoPlotBackground
Hide plot background surface.

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

### -NoProgressHandles
Hide progress handles.

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

### -Palette
Palette colors used by series and point-based charts.

```yaml
Type: ChartColor[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PictorialColumns
Pictorial columns per row.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PictorialSymbol
Pictorial symbol.

```yaml
Type: ChartPictorialSymbol
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Circle, Square, Diamond, Triangle, Star, Person

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PieLabelContent
Pie and donut slice label content.

```yaml
Type: ChartPieLabelContent
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Label, Value, Percent, LabelAndValue, LabelAndPercent

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ProgressMaximum
Progress maximum.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShowDataLabels
Show data labels.

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

### -ShowLegend
Show the legend.

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

### -ShowPointLegend
Show point-level legend entries.

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

### -TickCount
Preferred number of axis ticks.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Transparent
Use transparent background.

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

### -WordCloudMaximumTerms
Maximum number of word cloud terms.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ImagePlayground.ChartRenderOptions` — Renderer-neutral chart options exposed by ImagePlayground.

## RELATED LINKS

- None
