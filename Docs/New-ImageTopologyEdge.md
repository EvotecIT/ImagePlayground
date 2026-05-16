---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyEdge
## SYNOPSIS
Creates a topology edge definition.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyEdge [-SourceNodeId] <string> [-TargetNodeId] <string> [[-Label] <string>] [-Id <string>] [-SecondaryLabel <string>] [-TertiaryLabel <string>] [-Kind <TopologyEdgeKind>] [-Status <TopologyHealthStatus>] [-Direction <TopologyDirection>] [-Routing <TopologyEdgeRouting>] [-LineStyle <TopologyEdgeLineStyle>] [-Emphasis <TopologyEdgeEmphasis>] [-SourcePort <TopologyEdgePort>] [-TargetPort <TopologyEdgePort>] [-RouteLane <double>] [-LabelOffsetX <double>] [-LabelOffsetY <double>] [-Color <string>] [-Muted] [-Href <string>] [-Tooltip <string>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageTopology to describe a relationship between topology nodes.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopologyEdge -SourceNodeId api -TargetNodeId database -Label '32 ms' -Kind Dependency -Status Warning -Direction Forward
```

Creates a ChartForgeX topology edge that can be rendered by New-ImageTopology.

## PARAMETERS

### -Color
Optional edge color as a CSS hex color.

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

### -Direction
Direction marker behavior.

```yaml
Type: TopologyDirection
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Forward, Backward, Bidirectional

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Emphasis
Edge visual emphasis.

```yaml
Type: TopologyEdgeEmphasis
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Normal, Subtle, Strong

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

### -Id
Stable edge identifier. When omitted, a unique identifier is derived from source and target node ids.

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

### -Kind
Relationship kind.

```yaml
Type: TopologyEdgeKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Generic, Link, Replication, Connectivity, Dependency, Trust, Mapping, AuthenticationPath, CertificateChain, DataFlow, Ownership, Membership

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Label
Primary edge label.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -LabelOffsetX
Optional horizontal edge-label offset in pixels.

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

### -LabelOffsetY
Optional vertical edge-label offset in pixels.

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

### -LineStyle
Edge line style.

```yaml
Type: TopologyEdgeLineStyle
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, Solid, Dashed, Dotted

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Muted
Render the edge as a quiet structural relationship.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RouteLane
Optional route lane offset in pixels.

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

### -Routing
Edge routing mode.

```yaml
Type: TopologyEdgeRouting
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Straight, Curved, Orthogonal, ObstacleAvoidingOrthogonal

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -SecondaryLabel
Secondary edge label.

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

### -SourceNodeId
Source node identifier.

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

### -SourcePort
Preferred source attachment side.

```yaml
Type: TopologyEdgePort
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, Top, Right, Bottom, Left

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Status
Relationship health or state.

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

### -TargetNodeId
Target node identifier.

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

### -TargetPort
Preferred target attachment side.

```yaml
Type: TopologyEdgePort
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, Top, Right, Bottom, Left

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -TertiaryLabel
Tertiary edge label.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Topology.TopologyEdge`

## RELATED LINKS

- None
