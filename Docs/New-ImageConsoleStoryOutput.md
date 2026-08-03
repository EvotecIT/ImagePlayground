---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryOutput
## SYNOPSIS
Creates a typed output step for an ImagePlayground console story.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryOutput [-Text] <string> [-Style <TerminalTextTone>] [<CommonParameters>]
```

## DESCRIPTION
Creates a typed output step for an ImagePlayground console story.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success
```

The semantic tone is rendered consistently across SVG, HTML, PNG, GIF, and APNG output.

## PARAMETERS

### -Style
Semantic output color.

```yaml
Type: TerminalTextTone
Parameter Sets: __AllParameterSets
Aliases: Tone
Possible values: Default, Muted, Accent, Success, Warning, Error

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
One or more lines of terminal output.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep`: Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.

## RELATED LINKS

- None
