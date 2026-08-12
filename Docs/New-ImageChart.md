---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChart
## SYNOPSIS
Creates an image chart from definitions.

## SYNTAX
### ScriptBlock (Default)
```powershell
New-ImageChart [-ChartsDefinition] <scriptblock> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <Object[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <ChartColor>] [-Options <ChartRenderOptions>] [-Watermark <VisualWatermark[]>] [-Dpi <double>] [-PassThru] [<CommonParameters>]
```

### ChartScript
```powershell
New-ImageChart -ChartScript <scriptblock> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <Object[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <ChartColor>] [-Options <ChartRenderOptions>] [-Watermark <VisualWatermark[]>] [-Dpi <double>] [-PassThru] [<CommonParameters>]
```

### Chart
```powershell
New-ImageChart -Chart <Chart> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <Object[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <ChartColor>] [-Options <ChartRenderOptions>] [-Watermark <VisualWatermark[]>] [-Dpi <double>] [-PassThru] [<CommonParameters>]
```

### Definition
```powershell
New-ImageChart -Definition <Object[]> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <Object[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <ChartColor>] [-Options <ChartRenderOptions>] [-Watermark <VisualWatermark[]>] [-Dpi <double>] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render one or more chart definitions into a final image file.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChart -ChartsDefinition {
    New-ImageChartBar -Name 'Q1' -Value 12,18,25 -Color CornflowerBlue
    New-ImageChartBar -Name 'Q2' -Value 14,20,28 -Color Orange
} -FilePath chart.png -XTitle 'Month' -YTitle 'Revenue'
```

Builds chart definitions inside a script block and renders them into a PNG file.

### EXAMPLE 2
```powershell
PS> $defs = @(
    New-ImageChartLine -Name 'CPU' -Value 35,42,58,61,49 -Color LimeGreen -Smooth
)
$ann = @(
    New-ImageChartAnnotation -X 3 -Y 61 -Text 'Peak' -Arrow
)
New-ImageChart -Definition $defs -Annotation $ann -FilePath cpu-usage.png -Theme Dark -ShowGrid -Show
```

Renders a themed line chart and overlays an annotation highlighting the peak value.

## PARAMETERS

### -Annotation
Annotations for the chart.

```yaml
Type: Object[]
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AnnotationsDefinition
ScriptBlock producing annotations.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Background
Chart background color.

```yaml
Type: ChartColor
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Chart
ChartForgeX chart object to render.

```yaml
Type: Chart
Parameter Sets: Chart
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ChartScript
The script block receives the chart as its first argument and can mutate it directly or return a replacement chart.

```yaml
Type: ScriptBlock
Parameter Sets: ChartScript
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ChartsDefinition
ScriptBlock producing chart definitions.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Definition
Chart definitions provided directly.

```yaml
Type: Object[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Dpi
PNG physical resolution metadata in dots per inch.

```yaml
Type: Double
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Height of the chart.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Options
Renderer options created by New-ImageChartOptions.

```yaml
Type: ChartRenderOptions
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the ChartForgeX chart to the pipeline after rendering.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShowGrid
Display grid lines.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Theme
Chart theme.

```yaml
Type: ChartTheme
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values: Default, Dark, Light, Colorblind, Aurora, Editorial, Candy, PeopleInfographic, Terminal, TransparentOverlayDark, Minimal, DashboardLight, SaasDashboardLight, RestaurantDashboardLight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Watermark
Watermarks applied to static SVG, HTML, or PNG output.

```yaml
Type: VisualWatermark[]
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Width of the chart.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -XTitle
X axis title.

```yaml
Type: String
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -YTitle
Y axis title.

```yaml
Type: String
Parameter Sets: ScriptBlock, ChartScript, Chart, Definition
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

- `ChartForgeX.Core.Chart`
- `System.Object[]`

## OUTPUTS

- `ChartForgeX.Core.Chart`

## RELATED LINKS

- None
