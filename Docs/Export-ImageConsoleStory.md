---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Export-ImageConsoleStory
## SYNOPSIS
Exports an authored ImagePlayground console story to SVG, HTML, PNG, GIF, or APNG.

## SYNTAX
### __AllParameterSets
```powershell
Export-ImageConsoleStory [-Path] <string> -Story <TerminalStory> [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Use New-ImageConsoleStory to compose the reusable story once, then pipe it to this cmdlet for each required format. Export never executes displayed commands.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -Content {
  New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
  New-ImageConsoleStoryCommand -Text 'dotnet build'
  New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success
  New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
  New-ImageConsoleStoryCommand -Text './build.sh'
  New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success
}
$story | Export-ImageConsoleStory -Path '.\demo.gif'
```

Creates a Discord-friendly animated GIF while retaining the story object for other exports.

## PARAMETERS

### -AnimationScale
Raster density multiplier used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -EndHoldSeconds
Completed-state hold time used for animated GIF and APNG output.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: 1.2
Accept pipeline input: False
Accept wildcard characters: False
```

### -FramesPerSecond
Frame rate used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: 10
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaximumFrames
Maximum frame budget used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: 240
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoLoop
Produce a single-play animated GIF or APNG instead of a repeating animation.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the exported story back to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Output file path. Supported extensions are SVG, HTML, HTM, PNG, GIF, and APNG.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: FilePath
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the generated presentation after export.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Story
Authored terminal story to export. Accepts pipeline input from New-ImageConsoleStory.

```yaml
Type: TerminalStory
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Terminal.TerminalStory`

## OUTPUTS

- `ChartForgeX.Terminal.TerminalStory`

## RELATED LINKS

- None
