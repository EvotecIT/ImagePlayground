---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Select-ImageConsoleStoryTab
## SYNOPSIS
Switches an ImagePlayground console story to a previously declared persistent tab.

## SYNTAX
### __AllParameterSets
```powershell
Select-ImageConsoleStoryTab [-Id] <string> [-TransitionSeconds <double>] [<CommonParameters>]
```

## DESCRIPTION
Switches an ImagePlayground console story to a previously declared persistent tab.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Select-ImageConsoleStoryTab -Id main
```

Activates the initial tab without clearing its existing transcript buffer.

## PARAMETERS

### -Id
Identifier of a tab declared earlier in the story. The initial tab is named main.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TransitionSeconds
Duration of the visual switch to the selected tab.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 0,2
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep` — Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.

## RELATED LINKS

- None
