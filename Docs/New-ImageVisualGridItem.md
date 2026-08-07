---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageVisualGridItem
## SYNOPSIS
Creates one chart or visual-block placement for a visual grid.

## SYNTAX
### Block (Default)
```powershell
New-ImageVisualGridItem [-Block] <IVisualBlock> [-TargetId <string>] [-ColumnSpan <int>] [-RowSpan <int>] [<CommonParameters>]
```

### Chart
```powershell
New-ImageVisualGridItem [-Chart] <Chart> [-TargetId <string>] [-ColumnSpan <int>] [-RowSpan <int>] [<CommonParameters>]
```

## DESCRIPTION
Creates one chart or visual-block placement for a visual grid.

## EXAMPLES

### EXAMPLE 1
```powershell
$card = New-ImageMetricCard -Label 'Requests' -Value 12840 -Trend '+12%'
New-ImageVisualGridItem -TargetId 'requests' -Block $card -ColumnSpan 2
```


## PARAMETERS

### -Block
Visual block hosted by the grid item.

```yaml
Type: IVisualBlock
Parameter Sets: Block
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Chart
Chart hosted by the grid item.

```yaml
Type: Chart
Parameter Sets: Chart
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ColumnSpan
Number of grid columns occupied by the item.

```yaml
Type: Int32
Parameter Sets: Block, Chart
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RowSpan
Number of grid rows occupied by the item.

```yaml
Type: Int32
Parameter Sets: Block, Chart
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetId
Optional stable motion target identifier.

```yaml
Type: String
Parameter Sets: Block, Chart
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

- `ChartForgeX.VisualBlocks.VisualGridItem`

## RELATED LINKS

- None
