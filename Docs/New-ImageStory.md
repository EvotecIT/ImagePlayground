---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageStory
## SYNOPSIS
Creates a generic source-to-result visual story from resolved scenes and declared outcomes.

## SYNTAX
### Parts (Default)
```powershell
New-ImageStory -Title <string> -Scenes <ImageStorySceneSpec[]> -Outcomes <ImageStoryOutcomeSpec[]> -FilePath <string> [-Description <string>] [-Width <int>] [-Height <int>] [-Theme <VisualStoryTheme>] [-BundlePath <string>] [-BundleFormats <string[]>] [-CapturedAtUtc <DateTimeOffset>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-TransitionSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Story
```powershell
New-ImageStory -Story <VisualStory> -FilePath <string> [-BundlePath <string>] [-BundleFormats <string[]>] [-CapturedAtUtc <DateTimeOffset>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-TransitionSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
The cmdlet never executes displayed code. Callers or trusted build tooling capture real outputs first, then pass the resolved source, transcript, and media into the story.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $source = ConvertTo-ImageStorySource -Text $code -Language PowerShell
$codePanel = New-ImageStoryPanel -Id source -Source $source -Title 'PowerShell'
$chartPanel = New-ImageStoryPanel -Id chart -MediaPath .\chart.png -AccessibleText 'Weekly builds chart' -Title 'Result'
$scenes = @(
  New-ImageStoryScene -Id source -Title 'Write the script' -Panels $codePanel
  New-ImageStoryScene -Id result -Title 'See the chart' -Layout Split -Panels @($codePanel, $chartPanel)
)
$outcome = New-ImageStoryOutcome -Id chart-created -Label 'The chart is visible' -PanelId chart
New-ImageStory -Title 'Chart in five lines' -Scenes $scenes -Outcomes $outcome -FilePath .\chart-story.gif
```

Produces a portable animated story whose final scene contains the promised chart.

## PARAMETERS

### -AnimationScale
Raster density multiplier for GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -BundleFormats
Formats emitted into BundlePath. The completed PNG is always included.

```yaml
Type: String[]
Parameter Sets: Parts, Story
Aliases:
Possible values: Svg, Html, Png, Gif, Apng, Transcript

Required: False
Position: named
Default value: Svg, Png, Gif, Transcript
Accept pipeline input: False
Accept wildcard characters: False
```

### -BundlePath
Optional directory that receives a portable PowerForge-compatible story bundle and manifest.

```yaml
Type: String
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CapturedAtUtc
Optional capture time recorded in the portable bundle. Omit it for deterministic bundle manifests.

```yaml
Type: Nullable`1
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Accessible story description.

```yaml
Type: String
Parameter Sets: Parts
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EndHoldSeconds
Completed-scene hold time for GIF and APNG output.

```yaml
Type: Double
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: 1.5
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Output path. Supports SVG, HTML, PNG, GIF, APNG, and TXT.

```yaml
Type: String
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FramesPerSecond
Frame rate for GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: 6
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Logical output height.

```yaml
Type: Int32
Parameter Sets: Parts
Aliases:
Possible values:

Required: False
Position: named
Default value: 675
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaximumFrames
Maximum animated frame budget.

```yaml
Type: Int32
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: 240
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoLoop
Produce a single-play GIF or APNG.

```yaml
Type: SwitchParameter
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Outcomes
Outcomes that the completed scene must reveal.

```yaml
Type: ImageStoryOutcomeSpec[]
Parameter Sets: Parts
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the resolved native story to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scenes
Resolved scenes in display order.

```yaml
Type: ImageStorySceneSpec[]
Parameter Sets: Parts
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the generated story after creation.

```yaml
Type: SwitchParameter
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Story
Native resolved ChartForgeX visual story.

```yaml
Type: VisualStory
Parameter Sets: Story
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Theme
Optional visual-story theme.

```yaml
Type: VisualStoryTheme
Parameter Sets: Parts
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Story title.

```yaml
Type: String
Parameter Sets: Parts
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TransitionSeconds
Cross-fade duration between scenes.

```yaml
Type: Double
Parameter Sets: Parts, Story
Aliases:
Possible values:

Required: False
Position: named
Default value: 0.24
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Logical output width.

```yaml
Type: Int32
Parameter Sets: Parts
Aliases:
Possible values:

Required: False
Position: named
Default value: 1200
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Stories.VisualStory`

## OUTPUTS

- `ChartForgeX.Stories.VisualStory`

## RELATED LINKS

- None
