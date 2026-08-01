---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryTab
## SYNOPSIS
Declares a persistent tab in an ImagePlayground console story.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryTab [-Id] <string> [[-Title] <string>] [-Profile <string>] [-WorkingDirectory <string>] [-Active] [-Palette <TerminalTheme>] [-TransitionSeconds <double>] [<CommonParameters>]
```

## DESCRIPTION
Each tab owns its title, prompt dialect, working directory, palette, icon, and transcript buffer. Use Active for the initial tab and Select-ImageConsoleStoryTab for later visible switches.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
```

Names and styles the initial persistent tab without creating an extra transition.

### EXAMPLE 2
```powershell
PS> New-ImageConsoleStoryTab -Id ubuntu -Profile Ubuntu -Title Ubuntu -WorkingDirectory '~/src'
```

Creates a typed story step that opens and activates an Ubuntu-styled Bash session.

## PARAMETERS

### -Active
Configure this declaration as the initial active tab. It must be the first content step.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Stable identifier used by later tab-selection steps.

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

### -Palette
Optional custom palette, normally created by New-ImageConsoleStoryPalette.

```yaml
Type: TerminalTheme
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Profile
Built-in profile that supplies prompt behavior, default working directory, palette, and icon.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values: PowerShell, WindowsPowerShell, Ubuntu, Bash, CommandPrompt

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Visible tab title. Defaults to the selected profile name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TransitionSeconds
Duration of the visual switch from the previously active tab.

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

### -WorkingDirectory
Working directory shown by this tab. Defaults to C:\ for Windows profiles and ~ for POSIX profiles.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
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

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep` — Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.

## RELATED LINKS

- None
