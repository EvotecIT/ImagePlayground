---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryCommand
## SYNOPSIS
Creates a typed command step for an ImagePlayground console story.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryCommand [-Text] <string> [-DurationSeconds <double>] [<CommonParameters>]
```

## DESCRIPTION
Creates a typed command step for an ImagePlayground console story.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStoryCommand -Text 'Get-Service -Name WinRM'
```

The command is presented but never executed.

## PARAMETERS

### -DurationSeconds
Explicit typing duration in seconds, or zero to use automatic timing.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
Command text shown after the configured terminal prompt.

```yaml
Type: String
Parameter Sets: __AllParameterSets
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

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep`: Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.

## RELATED LINKS

- None
