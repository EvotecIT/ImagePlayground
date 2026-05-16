---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyNode
## SYNOPSIS
Creates a topology node definition.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyNode [-Id] <string> [-Label] <string> [-Subtitle <string>] [-Kind <TopologyNodeKind>] [-Status <TopologyHealthStatus>] [-GroupId <string>] [-X <double>] [-Y <double>] [-Longitude <double>] [-Latitude <double>] [-Width <double>] [-Height <double>] [-Symbol <string>] [-IconId <string>] [-DisplayMode <TopologyNodeDisplayMode>] [-Badge <string>] [-Color <string>] [-BackgroundColor <string>] [-Href <string>] [-Tooltip <string>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageTopology to describe a node in a topology diagram.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopologyNode -Id api -Label 'API' -Kind Service -Status Healthy -Symbol API
```

Creates a ChartForgeX topology node that can be rendered by New-ImageTopology.

## PARAMETERS

### -BackgroundColor
Optional node fill color as a CSS hex color.

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

### -Badge
Optional short badge text.

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

### -Color
Optional node accent color as a CSS hex color.

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

### -DisplayMode
Optional node display mode override.

```yaml
Type: TopologyNodeDisplayMode
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Card, CompactCard, Tile, Pill, Icon, Artwork, Dot, Hidden

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -GroupId
Optional parent group identifier.

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
Node height in pixels.

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
Stable node identifier used by topology edges.

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

### -Kind
Node kind used for icon and legend selection.

```yaml
Type: TopologyNodeKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Generic, Group, Location, Hub, Branch, Server, Service, Endpoint, Gateway, Cloud, Database, Storage, Network, NetworkSegment, Namespace, Application, Process, Queue, Person, Team, Certificate

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Label
Node label rendered in the diagram.

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
Optional node latitude for geographic layouts.

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

### -Longitude
Optional node longitude for geographic layouts.

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
Node health or state.

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
Optional node subtitle.

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
Short symbol rendered inside or near the node icon.

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
Node width in pixels.

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

- `ChartForgeX.Topology.TopologyNode`

## RELATED LINKS

- None
