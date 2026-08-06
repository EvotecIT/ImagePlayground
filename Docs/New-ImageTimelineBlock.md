---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTimelineBlock
## SYNOPSIS
Creates an activity timeline block.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTimelineBlock [-Item <ActivityTimelineItem[]>] [-ItemDefinition <scriptblock>] [-Title <string>] [-Subtitle <string>] [-Compact] [-Theme <ChartTheme>] [<CommonParameters>]
```

## DESCRIPTION
Creates an activity timeline block.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTimelineBlock -Title 'Release activity' -ItemDefinition { New-ImageTimelineItem -Kind Event -Title 'Build completed' -Status Positive; New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests' -Completed }
```


## PARAMETERS

### -Compact
Render event rows without card-like surfaces.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Item
Timeline items supplied directly.

```yaml
Type: ActivityTimelineItem[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ItemDefinition
Script block that emits items from New-ImageTimelineItem.

```yaml
Type: ScriptBlock
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Subtitle
Optional block subtitle.

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

### -Theme
ChartForgeX theme.

```yaml
Type: ChartTheme
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Default, Dark, Light, Colorblind, Aurora, Editorial, Candy, PeopleInfographic, Terminal, TransparentOverlayDark, Minimal, DashboardLight, SaasDashboardLight, RestaurantDashboardLight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Optional block title.

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

- `None`

## OUTPUTS

- `ChartForgeX.VisualBlocks.ActivityTimelineBlock`

## RELATED LINKS

- None
