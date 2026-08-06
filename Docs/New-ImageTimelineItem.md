---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTimelineItem
## SYNOPSIS
Creates one activity timeline item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTimelineItem [-Kind] <ActivityTimelineItemKind> [-Title] <string> [-Timestamp <string>] [-Status <VisualStatus>] [-Badge <string>] [-Detail <string>] [-Symbol <string>] [-Completed] [-Muted] [-HiddenCount <int>] [<CommonParameters>]
```

## DESCRIPTION
Creates one activity timeline item.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTimelineItem -Kind Event -Title 'Production deployed' -Timestamp '14:32' -Status Positive
```


## PARAMETERS

### -Badge
Optional compact event badge.

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

### -Completed
Mark a checklist item as completed.

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

### -Detail
Optional event detail.

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

### -HiddenCount
Number of collapsed items represented by a hidden summary.

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

### -Kind
Timeline item kind.

```yaml
Type: ActivityTimelineItemKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Section, Event, ChecklistItem, HiddenSummary

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Muted
Render checklist text as muted.

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

### -Status
Event status.

```yaml
Type: VisualStatus
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Neutral, Positive, Warning, Negative, Info

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Symbol
Optional compact event symbol.

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

### -Timestamp
Optional timestamp text for events.

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

### -Title
Primary item text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.VisualBlocks.ActivityTimelineItem`

## RELATED LINKS

- None
