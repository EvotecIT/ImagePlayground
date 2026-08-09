---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageListBlock
## SYNOPSIS
Creates a visual list block.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageListBlock [-Item] <string[]> [-Value <string[]>] [-Status <VisualStatus[]>] [-Checked <bool[]>] [-Title <string>] [-Subtitle <string>] [-Marker <VisualListMarker>] [-Dense] [-Theme <ChartTheme>] [<CommonParameters>]
```

## DESCRIPTION
Creates a visual list block.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageListBlock -Title 'Release' -Item Build,Test,Publish -Checked $true,$true,$false
```


## PARAMETERS

### -Checked
Optional checked states matching the item count.

```yaml
Type: Boolean[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Dense
Use compact row spacing.

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
List item text.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Marker
List marker style.

```yaml
Type: VisualListMarker
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Bullet, Number, Check, Status

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Status
Optional semantic statuses matching the item count.

```yaml
Type: VisualStatus[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Neutral, Positive, Warning, Negative, Info

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

### -Value
Optional right-aligned values matching the item count.

```yaml
Type: String[]
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

- `ChartForgeX.VisualBlocks.ChartList`

## RELATED LINKS

- None
