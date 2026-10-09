---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageStoryReplay
## SYNOPSIS
Converts ordered capture records into a resolved replay without executing commands.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageStoryReplay -DurationSeconds <double> -InputObject <psobject[]> [-Dialect <TerminalDialect>] [-WorkingDirectory <string>] [-Title <string>] [-CustomPrompt <string>] [<CommonParameters>]
```

## DESCRIPTION
Each event supplies TimestampSeconds and Kind. Command, Output, ReplaceLine, Directory and Marker use Text. OpenTab uses TabId, Title, Dialect, WorkingDirectory and CustomPrompt; SelectTab uses TabId. Output and ReplaceLine can supply Tone. Capture scripts remain a separate, explicit caller action.

## EXAMPLES

### EXAMPLE 1
```powershell
$events = @(
    @{ TimestampSeconds = 1; Kind = 'Command'; Text = './Test-Project.ps1' }
    @{ TimestampSeconds = 12; Kind = 'Output'; Text = '24 checks passed'; Tone = 'Success' }
)
$replay = New-ImageStoryReplay -DurationSeconds 15 -Events $events
$replay.CompressPauses([TimeSpan]::FromSeconds(2))
```


## PARAMETERS

### -CustomPrompt
Prompt required by the Custom dialect.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Dialect
Initial prompt dialect.

```yaml
Type: TerminalDialect
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: PowerShell, Bash, CommandPrompt, Python, CSharp, Custom

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DurationSeconds
Recorded endpoint, including idle time.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
Ordered dictionaries or objects, including objects read with ConvertFrom-Json.

```yaml
Type: PSObject[]
Parameter Sets: __AllParameterSets
Aliases: Events
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Title
Initial session title.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkingDirectory
Initial prompt directory.

```yaml
Type: String
Parameter Sets: __AllParameterSets
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

- `System.Management.Automation.PSObject[]`

## OUTPUTS

- `ChartForgeX.Stories.StoryReplay`

## RELATED LINKS

- None
