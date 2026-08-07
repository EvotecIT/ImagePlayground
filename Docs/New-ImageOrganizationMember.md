---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageOrganizationMember
## SYNOPSIS
Creates one member for an organization or team chart.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageOrganizationMember [-Id] <string> [-Name] <string> [[-Role] <string>] [-ParentId <string>] [-Level <int>] [-Status <TopologyHealthStatus>] [-LayoutPolicy <TopologyHierarchyLayoutPolicy>] [-IconId <string>] [-Metadata <hashtable>] [<CommonParameters>]
```

## DESCRIPTION
The returned ChartForgeX team member can be passed to New-ImageOrganizationChart.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageOrganizationMember -Id lead -Name 'Avery Stone' -Role 'Engineering Lead' -Status Healthy
```


## PARAMETERS

### -IconId
Optional ChartForgeX topology icon identifier.

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

### -Id
Stable member identifier.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LayoutPolicy
Sibling layout applied to this member's direct reports and inherited by its subtree.

```yaml
Type: TopologyHierarchyLayoutPolicy
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, Standard, Compact, Vertical

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Level
Optional explicit hierarchy level.

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

### -Metadata
Optional host-readable metadata copied to the generated topology node.

```yaml
Type: Hashtable
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Member display name.

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

### -ParentId
Manager or parent member identifier.

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

### -Role
Optional role or job title.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Status
Member health or availability state.

```yaml
Type: TopologyHealthStatus
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Healthy, Warning, Critical, Unknown, Disabled

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

- `ChartForgeX.Topology.TopologyTeamMember`

## RELATED LINKS

- None
