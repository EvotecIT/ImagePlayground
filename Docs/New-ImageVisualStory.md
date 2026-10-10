---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageVisualStory
## SYNOPSIS
Creates a script-free animated visual story from a ChartForgeX visual grid.

## SYNTAX
### StoryScript (Default)
```powershell
New-ImageVisualStory [-StoryScript] <scriptblock> -FilePath <string> [-Motion <VisualMotionTimeline>] [-MotionDefinition <scriptblock>] [-Show] [-PassThru] [<CommonParameters>]
```

### Grid
```powershell
New-ImageVisualStory -Grid <VisualGrid> -FilePath <string> [-Motion <VisualMotionTimeline>] [-MotionDefinition <scriptblock>] [-Show] [-PassThru] [<CommonParameters>]
```

### Presentation
```powershell
New-ImageVisualStory -Presentation <VisualMotionPresentation> -FilePath <string> [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Use a native ChartForgeX VisualGrid or configure one in StoryScript. SVG and HTML preserve motion, while PNG renders the completed static state.

A VisualMotionPresentation returned by New-ImageVisualGrid -Motion can also be piped directly into this command.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageVisualStory -StoryScript {
  param($Story)
  [void] $Story.WithTitle('Engineering signal').WithColumns(1)
  $metric = [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Projects', '126')
  [void] $Story.Add('projects', $metric)
} -MotionDefinition {
  New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.9
  New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.3
} -FilePath profile.svg
```

Builds a dependency-free SVG whose one-shot motion honors reduced-motion preferences.

### EXAMPLE 2
```powershell
PS> $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
New-ImageVisualGrid -ContentDefinition {
  New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840)
} -Motion $motion | New-ImageVisualStory -FilePath requests.svg
```

Reuses the typed presentation captured by the grid command without changing its source grid or timeline.

## PARAMETERS

### -FilePath
Output file path. Supported extensions are SVG, HTML, HTM, and PNG.

```yaml
Type: String
Parameter Sets: StoryScript, Grid, Presentation
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Grid
Native ChartForgeX visual grid to render.

```yaml
Type: VisualGrid
Parameter Sets: Grid
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Motion
Ready-to-use ChartForgeX motion timeline.

```yaml
Type: VisualMotionTimeline
Parameter Sets: StoryScript, Grid
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MotionDefinition
Script block that emits New-ImageVisualMotionCue results or one VisualMotionTimeline.

```yaml
Type: ScriptBlock
Parameter Sets: StoryScript, Grid
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the detached motion presentation, or the static grid when no motion is supplied.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Grid, Presentation
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Presentation
Detached motion presentation to export, including output from New-ImageVisualGrid -Motion.

```yaml
Type: VisualMotionPresentation
Parameter Sets: Presentation
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Show
Open the generated visual after creation.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Grid, Presentation
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StoryScript
Script block that receives and configures a new ChartForgeX VisualGrid.

```yaml
Type: ScriptBlock
Parameter Sets: StoryScript
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.VisualBlocks.VisualGrid`
- `ChartForgeX.Motion.VisualMotionPresentation`

## OUTPUTS

- `ChartForgeX.VisualBlocks.VisualGrid`
- `ChartForgeX.Motion.VisualMotionPresentation`

## RELATED LINKS

- None
