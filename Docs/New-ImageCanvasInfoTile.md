---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageCanvasInfoTile
## SYNOPSIS
Creates a positioned information tile for a visual canvas.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageCanvasInfoTile [-X] <double> [-Y] <double> [-Width] <double> [-Height] <double> [-Icon] <string> [-Label] <string> [-Value] <string> [-Detail <string>] [-Accent <ChartColor>] [-SurfaceStyle <VisualCanvasInfoTileSurfaceStyle>] [-IconKind <VisualCanvasInfoTileIconKind>] [-MiniChartKind <VisualCanvasInfoTileMiniChartKind>] [-MiniValues <double[]>] [-MiniChartMaximum <double>] [-Progress <double>] [-TextFitPolicy <VisualCanvasTextFitPolicy>] [<CommonParameters>]
```

## DESCRIPTION
Creates a positioned information tile for a visual canvas.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageCanvasInfoTile -X 72 -Y 180 -Width 360 -Height 150 -Icon API -Label 'Requests' -Value '12,840' -Detail '+12%' -MiniChartKind Area -MiniValues 8,9,10,12
```


## PARAMETERS

### -Accent
Optional accent color.

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

### -Detail
Optional supporting detail.

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
Tile height.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Icon
Compact icon or symbol.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IconKind
Built-in icon treatment.

```yaml
Type: VisualCanvasInfoTileIconKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Text, Computer, Network, OperatingSystem, Cpu, Memory, User, Domain, Terminal, Storage, Shield

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Label
Tile label.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MiniChartKind
Compact chart treatment.

```yaml
Type: VisualCanvasInfoTileMiniChartKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Sparkline, Area, Bars

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MiniChartMaximum
Optional compact chart maximum.

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

### -MiniValues
Values rendered by the compact chart.

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

### -Progress
Optional progress from zero to one.

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

### -SurfaceStyle
Tile surface treatment.

```yaml
Type: VisualCanvasInfoTileSurfaceStyle
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Glass, Outline, Raised

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TextFitPolicy
Text fitting policy.

```yaml
Type: VisualCanvasTextFitPolicy
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, SingleLineEllipsis, Wrap, ShrinkToFit, WrapThenShrink

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Primary tile value.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Tile width.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -X
Horizontal position in canvas design pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Y
Vertical position in canvas design pixels.

```yaml
Type: Double
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

- `ChartForgeX.Composition.VisualCanvasInfoTileLayer`

## RELATED LINKS

- None
