---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageStoryPanel
## SYNOPSIS
Creates one resolved source, terminal, media, or text panel for a generic visual story.

## SYNTAX
### Source (Default)
```powershell
New-ImageStoryPanel -Id <string> -Source <StorySourceText> [-Title <string>] [-Weight <double>] [<CommonParameters>]
```

### SourceText
```powershell
New-ImageStoryPanel -Id <string> -SourceText <string> [-Title <string>] [-Weight <double>] [-Language <string>] [-Tokenizer <IStorySourceTokenizer>] [<CommonParameters>]
```

### Terminal
```powershell
New-ImageStoryPanel -Id <string> -Terminal <TerminalStory> -AccessibleText <string> [-Title <string>] [-Weight <double>] [<CommonParameters>]
```

### MediaPath
```powershell
New-ImageStoryPanel -Id <string> -MediaPath <string> -AccessibleText <string> [-Title <string>] [-Weight <double>] [<CommonParameters>]
```

### MediaBytes
```powershell
New-ImageStoryPanel -Id <string> -MediaBytes <byte[]> -AccessibleText <string> [-Title <string>] [-Weight <double>] [-MediaSvg <string>] [<CommonParameters>]
```

### MediaImage
```powershell
New-ImageStoryPanel -Id <string> -MediaImage <RgbaImage> -AccessibleText <string> [-Title <string>] [-Weight <double>] [-MediaSvg <string>] [<CommonParameters>]
```

### Text
```powershell
New-ImageStoryPanel -Id <string> -Text <string> [-Title <string>] [-Weight <double>] [-Emphasized] [<CommonParameters>]
```

## DESCRIPTION
Creates one resolved source, terminal, media, or text panel for a generic visual story.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageStoryPanel -Id 'summary' -Title 'Result' -Text 'Deployment completed.' -Emphasized
```


## PARAMETERS

### -AccessibleText
Accessible alternative for terminal or media content.

```yaml
Type: String
Parameter Sets: Terminal, MediaPath, MediaBytes, MediaImage
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Emphasized
Give text content stronger visual emphasis.

```yaml
Type: SwitchParameter
Parameter Sets: Text
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Stable panel identifier referenced by completed outcomes.

```yaml
Type: String
Parameter Sets: Source, SourceText, Terminal, MediaPath, MediaBytes, MediaImage, Text
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Language
Language identifier used for SourceText.

```yaml
Type: String
Parameter Sets: SourceText
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MediaBytes
Resolved raster image bytes to reveal.

```yaml
Type: Byte[]
Parameter Sets: MediaBytes
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MediaImage
Resolved RGBA image to reveal.

```yaml
Type: RgbaImage
Parameter Sets: MediaImage
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MediaPath
Raster image file to reveal.

```yaml
Type: String
Parameter Sets: MediaPath
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MediaSvg
Optional resolved SVG representation paired with MediaBytes or MediaImage.

```yaml
Type: String
Parameter Sets: MediaBytes, MediaImage
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Source
Exact source text with optional renderer-neutral syntax spans.

```yaml
Type: StorySourceText
Parameter Sets: Source
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -SourceText
Exact source text to tokenize or preserve.

```yaml
Type: String
Parameter Sets: SourceText
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Terminal
Resolved deterministic terminal presentation.

```yaml
Type: TerminalStory
Parameter Sets: Terminal
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Text
Explanatory prose or a short status.

```yaml
Type: String
Parameter Sets: Text
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Optional visible panel title.

```yaml
Type: String
Parameter Sets: Source, SourceText, Terminal, MediaPath, MediaBytes, MediaImage, Text
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tokenizer
Optional language tokenizer for SourceText.

```yaml
Type: IStorySourceTokenizer
Parameter Sets: SourceText
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Weight
Relative size in split or stacked scenes.

```yaml
Type: Double
Parameter Sets: Source, SourceText, Terminal, MediaPath, MediaBytes, MediaImage, Text
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

- `ChartForgeX.Stories.StorySourceText`
- `ChartForgeX.Terminal.TerminalStory`

## OUTPUTS

- `ImagePlayground.PowerShell.Stories.ImageStoryPanelSpec`: PowerShell-friendly resolved panel specification for New-ImageStoryScene.

## RELATED LINKS

- None
