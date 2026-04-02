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
### ScriptBlock
```powershell
New-ImageChart [-ChartsDefinition] <scriptblock> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <Color>] [<CommonParameters>]
```

### Definition
```powershell
New-ImageChart -Definition <ChartDefinition[]> -FilePath <string> [-AnnotationsDefinition <scriptblock>] [-Annotation <ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [-Background <Color>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render one or more chart definitions into a final image file.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChart -ChartsDefinition {
                New-ImageChartBar -Name 'Q1' -Value 12,18,25 -Color CornflowerBlue
                New-ImageChartBar -Name 'Q2' -Value 14,20,28 -Color Orange
            } -FilePath chart.png -XTitle 'Month' -YTitle 'Revenue'
```

Builds chart definitions inside a script block and renders them into a PNG file.

### EXAMPLE 2
```powershell
$defs = @(
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
Type: ChartAnnotation[]
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -AnnotationsDefinition
ScriptBlock producing annotations.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Background
Chart background color.

```yaml
Type: Nullable`1
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ChartsDefinition
ScriptBlock producing chart definitions.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Definition
Chart definitions provided directly.

```yaml
Type: ChartDefinition[]
Parameter Sets: Definition
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
Height of the chart.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 400
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Open the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShowGrid
Display grid lines.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Theme
Chart theme.

Possible values: Default, Dark, Light

```yaml
Type: ChartTheme
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: Default, Dark, Light

Required: False
Position: named
Default value: Default
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Width of the chart.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 600
Accept pipeline input: False
Accept wildcard characters: True
```

### -XTitle
X axis title.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -YTitle
Y axis title.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: 
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

- `ImagePlayground.ChartDefinition[]`

## OUTPUTS

- `None`

## RELATED LINKS

- None

