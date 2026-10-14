---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTableBlock
## SYNOPSIS
Creates a visual table block from PowerShell objects.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTableBlock [[-Property] <Object[]>] [-InputObject] <psobject[]> [-Header <string[]>] [-Align <IDictionary>] [-Title <string>] [-Subtitle <string>] [-StatusColumn <string>] [-Dense] [-NoHeader] [-NoRowStriping] [-Width <int>] [-Height <int>] [-Theme <ChartTheme>] [<CommonParameters>]
```

## DESCRIPTION
Pipe objects in and choose columns with -Property, the same way as Select-Object or Format-Table. Property accepts property names and calculated property hashtables with Name (or Label), Expression, FormatString, Alignment, and Width keys.

Rows can be PowerShell objects, hashtables, or arrays of values in column order. Without -Property, the columns come from the first row.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Get-Service | Select-Object -First 5 | New-ImageTableBlock -Property Name, Status -StatusColumn Status -Title Services
```

Creates one table block with a Name and a Status column. Missing properties produce empty cells.

### EXAMPLE 2
```powershell
PS> $services | New-ImageTableBlock -Title Services -StatusColumn Status -Width 616 -Height 270 -Property Name, Status,
                @{ Name = 'Requests'; Expression = { [int] $_.Requests }; FormatString = 'N0'; Alignment = 'Right' },
                @{ Name = 'P95'; Expression = { "$($_.P95) ms" }; Alignment = 'Right' }
```

Formats the request count with thousands separators, appends a unit to latency, and sizes the block for a dashboard grid.

### EXAMPLE 3
```powershell
PS> New-ImageTableBlock -Title Services -Column Name, Status -Row @{ Name = 'API'; Status = 'Healthy' }, @{ Name = 'Database'; Status = 'Warning' } -Dense
```

-Column and -Row remain available as aliases of -Property and -InputObject.

## PARAMETERS

### -Align
Column alignment keyed by property or displayed header. Values are Left, Center, or Right.

```yaml
Type: IDictionary
Parameter Sets: __AllParameterSets
Aliases: Alignment
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

### -Header
Displayed column names. Defaults to the property names.

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

### -InputObject
Rows can be objects, hashtables, or arrays of values in column order.

```yaml
Type: PSObject[]
Parameter Sets: __AllParameterSets
Aliases: Row, Rows
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -NoHeader
Hide the table header.

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

### -NoRowStriping
Disable alternating row surfaces.

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

### -Property
Calculated properties accept Name (or Label), Expression (a property name or a script block using $_), FormatString, Alignment (Left, Center, Right), and Width.

When omitted, the columns are the properties of the first row.

```yaml
Type: Object[]
Parameter Sets: __AllParameterSets
Aliases: Column, Columns
Possible values:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StatusColumn
Column, by property or displayed header, that contains semantic status values such as Healthy, Warning, or Failed.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: StatusProperty
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

- `System.Management.Automation.PSObject[]`

## OUTPUTS

- `ChartForgeX.VisualBlocks.ChartTable`

## RELATED LINKS

- None
