---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageOrganizationChart
## SYNOPSIS
Creates an organization or team chart.

## SYNTAX
### Definition (Default)
```powershell
New-ImageOrganizationChart [-MemberDefinition] <scriptblock> -TeamLabel <string> -FilePath <string> [-TeamId <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-MinLevel <int>] [-MaxLevel <int>] [-NoTeamNode] [-NoAncestorContext] [-ShowLegend] [-Theme <string>] [-Transparent] [-Show] [-PassThru] [<CommonParameters>]
```

### Member
```powershell
New-ImageOrganizationChart -Member <TopologyTeamMember[]> -TeamLabel <string> -FilePath <string> [-TeamId <string>] [-Subtitle <string>] [-Width <int>] [-Height <int>] [-Direction <TopologyLayoutDirection>] [-NodeDisplayMode <TopologyNodeDisplayMode>] [-MinLevel <int>] [-MaxLevel <int>] [-NoTeamNode] [-NoAncestorContext] [-ShowLegend] [-Theme <string>] [-Transparent] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Maps PowerShell-authored team members into the shared ChartForgeX hierarchy engine, including compact child buckets and orthogonal parent-child routing.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageOrganizationChart -TeamId engineering -TeamLabel 'Engineering' -MemberDefinition { New-ImageOrganizationMember -Id lead -Name 'Avery Stone' -Role 'Director'; New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId lead } -FilePath engineering.svg
```


## PARAMETERS

### -Direction
Layered organization-chart direction.

```yaml
Type: TopologyLayoutDirection
Parameter Sets: Definition, Member
Aliases: None
Possible values: TopToBottom, LeftToRight, BottomToTop, RightToLeft

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Output file path. Supported extensions are PNG, SVG, HTML, and HTM.

```yaml
Type: String
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Output height in pixels.

```yaml
Type: Int32
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaxLevel
Highest hierarchy level to include.

```yaml
Type: Int32
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Member
Members supplied directly or through the pipeline.

```yaml
Type: TopologyTeamMember[]
Parameter Sets: Member
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -MemberDefinition
Script block that emits members from New-ImageOrganizationMember.

```yaml
Type: ScriptBlock
Parameter Sets: Definition
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MinLevel
Lowest primary hierarchy level to include.

```yaml
Type: Int32
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoAncestorContext
Omit ancestors below the requested minimum level.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NodeDisplayMode
Organization member presentation mode.

```yaml
Type: TopologyNodeDisplayMode
Parameter Sets: Definition, Member
Aliases: None
Possible values: Card, CompactCard, Tile, Pill, Icon, Artwork, Dot, Hidden

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoTeamNode
Omit the generated team root node.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the generated ChartForgeX topology chart to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the generated organization chart.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShowLegend
Show the node-kind and status legend.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Subtitle
Optional chart subtitle.

```yaml
Type: String
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TeamId
Stable team root identifier.

```yaml
Type: String
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TeamLabel
Team or organization label.

```yaml
Type: String
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Theme
Topology theme name.

```yaml
Type: String
Parameter Sets: Definition, Member
Aliases: None
Possible values: Light, Dark

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Transparent
Use a transparent chart background.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Member
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Output width in pixels.

```yaml
Type: Int32
Parameter Sets: Definition, Member
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

- `ChartForgeX.Topology.TopologyTeamMember[]`

## OUTPUTS

- `ChartForgeX.Topology.TopologyChart`

## RELATED LINKS

- None
