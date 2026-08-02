---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryTab
## SYNOPSIS
Creates a persistent tab in an ImagePlayground console story.

## SYNTAX
### Open (Default)
```powershell
New-ImageConsoleStoryTab [-Id] <string> [[-Title] <string>] [-Profile <string>] [-WorkingDirectory <string>] [-Palette <TerminalTheme>] [-TransitionSeconds <double>] [<CommonParameters>]
```

### Initial
```powershell
New-ImageConsoleStoryTab [-Id] <string> [[-Title] <string>] -Active [-Profile <string>] [-WorkingDirectory <string>] [-Palette <TerminalTheme>] [-TransitionSeconds <double>] [<CommonParameters>]
```

### Background
```powershell
New-ImageConsoleStoryTab [-Id] <string> [[-Title] <string>] -Background [-Profile <string>] [-WorkingDirectory <string>] [-Palette <TerminalTheme>] [-TransitionSeconds <double>] [<CommonParameters>]
```

## DESCRIPTION
By default, the new tab opens and becomes active after the current tab's configured reading dwell. Use Active for the initial tab, Background to pre-stage a visible inactive tab, and Select-ImageConsoleStoryTab to revisit any existing tab without clearing its buffer.

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

Opens and activates an Ubuntu-styled Bash session only after the previous tab has finished its configured reading dwell.

### EXAMPLE 3
```powershell
PS> New-ImageConsoleStoryTab -Id logs -Title Logs -Profile PowerShell -Background
            Select-ImageConsoleStoryTab -Id logs
```

The declaration makes the tab available in the strip without interrupting the active session. Selection later activates its retained buffer.

## PARAMETERS

### -Active
Configure this declaration as the initial active tab. It must be the first content step.

```yaml
Type: SwitchParameter
Parameter Sets: Initial
Aliases: None
Possible values:

Required: True
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Background
Declare the tab without activating it. Use Select-ImageConsoleStoryTab for the later intentional switch.

```yaml
Type: SwitchParameter
Parameter Sets: Background
Aliases: None
Possible values:

Required: True
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Stable identifier used by later tab-selection steps.

```yaml
Type: String
Parameter Sets: Open, Initial, Background
Aliases: None
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
Parameter Sets: Open, Initial, Background
Aliases: None
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
Parameter Sets: Open, Initial, Background
Aliases: None
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
Parameter Sets: Open, Initial, Background
Aliases: None
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
Parameter Sets: Open, Initial, Background
Aliases: None
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
Parameter Sets: Open, Initial, Background
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

- `None`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep`: Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.

## RELATED LINKS

- None
