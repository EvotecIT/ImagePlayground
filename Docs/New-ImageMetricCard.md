---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageMetricCard
## SYNOPSIS
Creates a dashboard metric card.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageMetricCard [-Label] <string> [-Value] <Object> [-Format <string>] [-Unit <string>] [-Caption <string>] [-Trend <string>] [-Status <VisualStatus>] [-Icon <VisualIcon>] [-MiniValues <double[]>] [-MiniChart <string>] [-MiniColor <ChartColor>] [-Width <int>] [-Height <int>] [-Theme <ChartTheme>] [-NoCard] [-Transparent] [<CommonParameters>]
```

## DESCRIPTION
The returned ChartForgeX block can be composed by New-ImageVisualGrid or New-ImageVisualStory.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageMetricCard -Label 'Requests' -Value 12840 -Trend '+12%' -Status Positive -MiniValues 8200,9400,10100,12840
```


## PARAMETERS

### -Caption
Optional supporting caption.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Format
Optional numeric or date format string.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Card height in pixels.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Icon
Built-in metric icon.

```yaml
Type: VisualIcon
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, ForkKnife, Flame, Lightning, Droplet, Runner, Bicycle, Person

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Label
Metric label.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MiniChart
Mini chart kind.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Sparkline, Bars

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MiniColor
Optional mini chart color.

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

### -MiniValues
Values used by the optional mini chart.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoCard
Hide the card surface.

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

### -Status
Semantic metric status.

```yaml
Type: VisualStatus
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Neutral, Positive, Warning, Negative, Info

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Theme
ChartForgeX theme.

```yaml
Type: ChartTheme
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Default, Dark, Light, Colorblind, Aurora, Editorial, Candy, PeopleInfographic, Terminal, TransparentOverlayDark, Minimal, DashboardLight, SaasDashboardLight, RestaurantDashboardLight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Transparent
Use a transparent card background.

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

### -Trend
Optional trend text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Unit
Optional unit displayed beside the value.

```yaml
Type: String
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
Metric value.

```yaml
Type: Object
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Card width in pixels.

```yaml
Type: Int32
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

- `None`

## OUTPUTS

- `ChartForgeX.VisualBlocks.MetricCard`

## RELATED LINKS

- None
