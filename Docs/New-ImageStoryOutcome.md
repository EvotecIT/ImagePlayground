---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageStoryOutcome
## SYNOPSIS
Declares a result that must be visible in the completed visual-story scene.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageStoryOutcome -Id <string> -Label <string> -PanelId <string> [<CommonParameters>]
```

## DESCRIPTION
Declares a result that must be visible in the completed visual-story scene.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageStoryOutcome -Id 'chart-created' -Label 'The chart is visible' -PanelId 'chart'
```


## PARAMETERS

### -Id
Stable outcome identifier.

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

### -Label
Human-readable outcome label.

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

### -PanelId
Panel identifier that must be present in the completed scene.

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

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.Stories.ImageStoryOutcomeSpec`: PowerShell-friendly completed-outcome specification for New-ImageStory.

## RELATED LINKS

- None
