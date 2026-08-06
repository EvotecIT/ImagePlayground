---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageVisualGrid
## SYNOPSIS
Creates a reusable dashboard grid from charts and visual blocks.

## SYNTAX
### Definition (Default)
```powershell
New-ImageVisualGrid [-ContentDefinition] <scriptblock> [-Title <string>] [-Subtitle <string>] [-Columns <int>] [-Gap <int>] [-Padding <int>] [-PanelWidth <int>] [-PanelHeight <int>] [-PanelFit <VisualPanelFit>] [-AdaptiveRowHeights] [-Frame] [-Theme <ChartTheme>] [-Motion <VisualMotionTimeline>] [-FilePath <string>] [-Show] [-PassThru] [<CommonParameters>]
```

### Content
```powershell
New-ImageVisualGrid -Content <Object[]> [-Title <string>] [-Subtitle <string>] [-Columns <int>] [-Gap <int>] [-Padding <int>] [-PanelWidth <int>] [-PanelHeight <int>] [-PanelFit <VisualPanelFit>] [-AdaptiveRowHeights] [-Frame] [-Theme <ChartTheme>] [-Motion <VisualMotionTimeline>] [-FilePath <string>] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Render the grid directly, pass it to New-ImageVisualStory, or compose it inside a ChartForgeX visual canvas.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageVisualGrid -Title 'Service health' -Columns 2 -ContentDefinition { New-ImageMetricCard -Label 'Requests' -Value 12840 -Status Positive; New-ImageListBlock -Title 'Checks' -Item API,Database -Status Positive,Warning } -FilePath dashboard.svg
```


## PARAMETERS

### -AdaptiveRowHeights
Use each row's natural item height.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Columns
Preferred column count.

```yaml
Type: Int32
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Content
Charts, visual blocks, or grid items supplied directly or through the pipeline.

```yaml
Type: Object[]
Parameter Sets: Content
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ContentDefinition
Script block that emits charts, visual blocks, or New-ImageVisualGridItem results.

```yaml
Type: ScriptBlock
Parameter Sets: Definition
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Optional output path. Omit it to return the grid without rendering.

```yaml
Type: String
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Frame
Render a subtle outer frame.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Gap
Gap between panels in pixels.

```yaml
Type: Int32
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Motion
Optional script-free SVG and HTML motion timeline.

```yaml
Type: VisualMotionTimeline
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Padding
Outer grid padding in pixels.

```yaml
Type: Int32
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PanelFit
How child content fits fixed panels.

```yaml
Type: VisualPanelFit
Parameter Sets: Definition, Content
Aliases: None
Possible values: Contain, Stretch

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PanelHeight
Optional fixed panel height.

```yaml
Type: Int32
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PanelWidth
Optional fixed panel width.

```yaml
Type: Int32
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Return the grid when an output file is also written.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the rendered dashboard.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Content
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Subtitle
Optional dashboard subtitle.

```yaml
Type: String
Parameter Sets: Definition, Content
Aliases: None
Possible values:

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
Parameter Sets: Definition, Content
Aliases: None
Possible values: Default, Dark, Light, Colorblind, Aurora, Editorial, Candy, PeopleInfographic, Terminal, TransparentOverlayDark, Minimal, DashboardLight, SaasDashboardLight, RestaurantDashboardLight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Optional dashboard title.

```yaml
Type: String
Parameter Sets: Definition, Content
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

- `System.Object[]`

## OUTPUTS

- `ChartForgeX.VisualBlocks.VisualGrid`

## RELATED LINKS

- None
