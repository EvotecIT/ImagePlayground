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
New-ImageTopology [-TopologyDefinition] <scriptblock> -FilePath <string> [-ScenarioDefinition <scriptblock>] [-Title <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Padding <int>] [-Layout <TopologyLayoutMode>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-VisualStyle <TopologyVisualStyle>] [-CanvasSurfaceStyle <TopologyCanvasSurfaceStyle>] [-Theme <string>] [-Transparent] [-NoTitle] [-NoLegend] [-NoGroups] [-NoEdgeLabels] [-NoStatusBadges] [-FitContentToViewport] [-InteractiveHtml] [-ActiveScenarioId <string>] [-Motion <TopologyMotionOptions>] [-NoScenarioControls] [-ScenarioControlMode <TopologyHtmlScenarioControlMode>] [-NoScenarioPanel] [-ScenarioUrlState] [-Show] [-PassThru] [<CommonParameters>]
```

### Definition
```powershell
New-ImageTopology -FilePath <string> [-Definition <Object[]>] [-Chart <TopologyChart>] [-Node <TopologyNode[]>] [-Edge <TopologyEdge[]>] [-Group <TopologyGroup[]>] [-Scenario <TopologyScenario[]>] [-ScenarioDefinition <scriptblock>] [-Title <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Padding <int>] [-Layout <TopologyLayoutMode>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-VisualStyle <TopologyVisualStyle>] [-CanvasSurfaceStyle <TopologyCanvasSurfaceStyle>] [-Theme <string>] [-Transparent] [-NoTitle] [-NoLegend] [-NoGroups] [-NoEdgeLabels] [-NoStatusBadges] [-FitContentToViewport] [-InteractiveHtml] [-ActiveScenarioId <string>] [-Motion <TopologyMotionOptions>] [-NoScenarioControls] [-ScenarioControlMode <TopologyHtmlScenarioControlMode>] [-NoScenarioPanel] [-ScenarioUrlState] [-Show] [-PassThru] [<CommonParameters>]
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

### -ActiveScenarioId
Scenario activated for static highlighting or when interactive HTML first loads.

```yaml
Type: String
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
```

### -Motion
Script-free route motion used by SVG, GIF, APNG, and sampled PNG output.

```yaml
Type: TopologyMotionOptions
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
```

### -NoScenarioControls
Hide scenario controls in interactive HTML output.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoScenarioPanel
Hide the compact scenario detail panel in interactive HTML output.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
```

### -Scenario
Topology scenarios provided directly.

```yaml
Type: TopologyScenario[]
Parameter Sets: Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScenarioControlMode
Scenario control presentation used by interactive HTML output.

```yaml
Type: TopologyHtmlScenarioControlMode
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values: Buttons, Checkboxes

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScenarioDefinition
Script block that emits topology scenarios.

```yaml
Type: ScriptBlock
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ScenarioUrlState
Synchronize the active scenario with the HTML page query string.

```yaml
Type: SwitchParameter
Parameter Sets: ScriptBlock, Definition
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
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
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.Object[]`

## OUTPUTS

- `ChartForgeX.Topology.TopologyChart`

## RELATED LINKS

- None
