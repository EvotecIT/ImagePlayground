---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopology
## SYNOPSIS
Creates a topology diagram image.

## SYNTAX
### ScriptBlock (Default)
```powershell
New-ImageTopology [-TopologyDefinition] <scriptblock> -FilePath <string> [-Title <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Padding <int>] [-Layout <TopologyLayoutMode>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-VisualStyle <TopologyVisualStyle>] [-CanvasSurfaceStyle <TopologyCanvasSurfaceStyle>] [-Theme <string>] [-Transparent] [-NoTitle] [-NoLegend] [-NoGroups] [-NoEdgeLabels] [-NoStatusBadges] [-FitContentToViewport] [-InteractiveHtml] [-Show] [-PassThru] [<CommonParameters>]
```

### Definition
```powershell
New-ImageTopology -FilePath <string> [-Definition <Object[]>] [-Chart <TopologyChart>] [-Node <TopologyNode[]>] [-Edge <TopologyEdge[]>] [-Group <TopologyGroup[]>] [-Title <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Padding <int>] [-Layout <TopologyLayoutMode>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-VisualStyle <TopologyVisualStyle>] [-CanvasSurfaceStyle <TopologyCanvasSurfaceStyle>] [-Theme <string>] [-Transparent] [-NoTitle] [-NoLegend] [-NoGroups] [-NoEdgeLabels] [-NoStatusBadges] [-FitContentToViewport] [-InteractiveHtml] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Renders ChartForgeX topology definitions to PNG, SVG, or HTML output.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopology -TopologyDefinition {
    New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy -Symbol API
    New-ImageTopologyNode -Id db -Label Database -Kind Database -Status Warning -Symbol SQL
    New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -Label '32 ms' -Kind Dependency -Status Warning -Direction Forward
} -Title 'Service map' -Layout Layered -Direction LeftToRight -FilePath service-map.png
```

Creates a transparent-ready PNG topology diagram from a PowerShell DSL.

## PARAMETERS

### -CanvasSurfaceStyle
Canvas surface style.

```yaml
Type: TopologyCanvasSurfaceStyle
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Plain, Panel, PanelGrid

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Chart
Topology chart provided directly.

```yaml
Type: TopologyChart
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Definition
Topology objects provided directly or from the pipeline.

```yaml
Type: Object[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Direction
Topology layout direction.

```yaml
Type: TopologyLayoutDirection
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: TopToBottom, LeftToRight, BottomToTop, RightToLeft

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Edge
Topology edges provided directly.

```yaml
Type: TopologyEdge[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Output file path. The extension selects PNG, SVG, or HTML output.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FitContentToViewport
Scale topology content down to remain inside the requested viewport.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Group
Topology groups provided directly.

```yaml
Type: TopologyGroup[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
Viewport height in pixels.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -InteractiveHtml
Enable lightweight interactions for HTML output.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Layout
Topology layout mode.

```yaml
Type: TopologyLayoutMode
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Manual, GroupGrid, HubAndSpoke, Layered, Matrix, DenseGrouped, Geographic, ForceDirected, RelationshipRadial, MindMap

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Node
Topology nodes provided directly.

```yaml
Type: TopologyNode[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NodeDisplayMode
Node presentation mode.

```yaml
Type: TopologyNodeDisplayMode
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Card, CompactCard, Tile, Pill, Icon, Artwork, Dot, Hidden

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoEdgeLabels
Hide edge labels.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoGroups
Hide topology groups.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoLegend
Hide the topology legend.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoStatusBadges
Hide status badges.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -NoTitle
Hide the topology title.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Padding
Viewport padding in pixels.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PassThru
Write the generated topology chart to the pipeline after rendering.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Open the generated file after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Subtitle
Topology subtitle.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Theme
Topology theme name.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Light, Dark

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Title
Topology title.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -TopologyDefinition
Script block that emits topology groups, nodes, edges, or a topology chart.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Transparent
Use a transparent diagram canvas.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -VisualStyle
Reusable visual style.

```yaml
Type: TopologyVisualStyle
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Default, MonitoringDashboard

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Viewport width in pixels.

```yaml
Type: Int32
Parameter Sets: ScriptBlock, Definition
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

- `System.Object[]`

## OUTPUTS

- `ChartForgeX.Topology.TopologyChart`

## RELATED LINKS

- None
