---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyGroup
## SYNOPSIS
Creates a topology group definition.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyGroup [-Id] <string> [-Label] <string> [-Subtitle <string>] [-Status <TopologyHealthStatus>] [-X <double>] [-Y <double>] [-Width <double>] [-Height <double>] [-Longitude <double>] [-Latitude <double>] [-LayoutPolicy <TopologyGroupLayoutPolicy>] [-Symbol <string>] [-IconId <string>] [-Color <string>] [-Href <string>] [-Tooltip <string>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageTopology to describe a logical region or cluster.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopologyGroup -Id site-a -Label 'Site A' -Status Healthy -Symbol region
```

Creates a ChartForgeX topology group that can contain topology nodes.

## PARAMETERS

### -Color
Optional group accent color as a CSS hex color.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
Group height in pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Href
Optional drill-down link for SVG and HTML outputs.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -IconId
Reusable ChartForgeX icon identifier.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Id
Stable group identifier.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Label
Group label rendered in the diagram.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Latitude
Optional group latitude for geographic layouts.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -LayoutPolicy
Preferred node arrangement policy for dense grouped layouts.

```yaml
Type: TopologyGroupLayoutPolicy
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, HubAndBranch, Grid, PairRows, MiniMesh, CollapsedDots

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Longitude
Optional group longitude for geographic layouts.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Status
Group health or state.

```yaml
Type: TopologyHealthStatus
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Healthy, Warning, Critical, Unknown, Disabled

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Subtitle
Optional group subtitle.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Symbol
Short symbol rendered near the group header.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Tooltip
Optional tooltip for SVG and HTML outputs.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Group width in pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
Manual X coordinate. Automatic layouts can ignore this value.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
Manual Y coordinate. Automatic layouts can ignore this value.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Topology.TopologyGroup`

## RELATED LINKS

- None
