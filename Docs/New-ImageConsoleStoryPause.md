---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryPause
## SYNOPSIS
Creates a silent timeline pause for an ImagePlayground console story.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryPause [-Seconds] <double> [<CommonParameters>]
```

## DESCRIPTION
Creates a silent timeline pause for an ImagePlayground console story.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStoryPause -Seconds 0.8
```

The pause affects animated SVG, HTML, GIF, and APNG timelines.

## PARAMETERS

### -Seconds
Pause duration in seconds.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 0
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep` — Use the New-ImageConsoleStoryCommand, New-ImageConsoleStoryOutput, New-ImageConsoleStoryTable, New-ImageConsoleStoryBlankLine, and New-ImageConsoleStoryPause cmdlets to create steps.

## RELATED LINKS

- None
