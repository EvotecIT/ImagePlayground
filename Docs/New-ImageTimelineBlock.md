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
New-ImageTimelineBlock [-Item <ActivityTimelineItem[]>] [-ItemDefinition <scriptblock>] [-Title <string>] [-Subtitle <string>] [-Compact] [-Width <int>] [-Height <int>] [-Theme <ChartTheme>] [<CommonParameters>]
```

## DESCRIPTION
Creates an activity timeline block.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTimelineBlock -Title 'Release activity' -ItemDefinition { New-ImageTimelineItem -Kind Event -Title 'Build completed' -Status Positive; New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests' -Completed }
```


### EXAMPLE 2
```powershell
PS> $deployments | ForEach-Object { New-ImageTimelineItem -Kind Event -Title $_.Name -Timestamp $_.Time -Status Positive } | New-ImageTimelineBlock -Title Deployments -Width 420 -Height 300
```

Items can arrive through the pipeline; Width and Height size the block for a grid panel.

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

### -Height
Block height in pixels. The width keeps its default unless -Width is also supplied.

```yaml
Type: Int32
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
Timeline items supplied directly or through the pipeline.

```yaml
Type: ActivityTimelineItem[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: True (ByValue)
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

### -Width
Block width in pixels. The height keeps its default unless -Height is also supplied.

```yaml
Type: Int32
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

- `ChartForgeX.VisualBlocks.ActivityTimelineItem[]`

## OUTPUTS

- `ChartForgeX.VisualBlocks.ActivityTimelineBlock`

## RELATED LINKS

- None
