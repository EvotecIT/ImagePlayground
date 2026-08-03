---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageStoryScene
## SYNOPSIS
Groups resolved panels into one timed visual-story scene.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageStoryScene -Id <string> -Title <string> -Panels <ImageStoryPanelSpec[]> [-Layout <VisualStorySceneLayout>] [-DurationSeconds <double>] [<CommonParameters>]
```

## DESCRIPTION
Groups resolved panels into one timed visual-story scene.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageStoryScene
```

## PARAMETERS

### -DurationSeconds
Scene display duration.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: 2.5
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Stable scene identifier.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Layout
Panel arrangement.

```yaml
Type: VisualStorySceneLayout
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Focus, Split, Stacked

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Panels
Resolved panels in display order.

```yaml
Type: ImageStoryPanelSpec[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Title
Visible scene heading.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ImagePlayground.PowerShell.Stories.ImageStoryPanelSpec[]`: PowerShell-friendly resolved panel specification for New-ImageStoryScene.

## OUTPUTS

- `ImagePlayground.PowerShell.Stories.ImageStorySceneSpec`: PowerShell-friendly resolved scene specification for New-ImageStory.

## RELATED LINKS

- None
