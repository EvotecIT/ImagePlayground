---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageTopologyDiagnostics
## SYNOPSIS
Analyzes prepared ChartForgeX topology geometry and routing.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageTopologyDiagnostics [-Topology] <TopologyChart> [-LayoutPreset <TopologyLayoutPreset>] [<CommonParameters>]
```

## DESCRIPTION
Returns machine-readable node bounds, named port positions, edge routes, fallback reasons, overlap scores, and collisions.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageTopologyDiagnostics
```

## PARAMETERS

### -LayoutPreset
Reusable topology spacing and presentation profile.

```yaml
Type: TopologyLayoutPreset
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Automatic, Dense, Compact, Balanced, Presentation

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Topology
Topology chart to analyze.

```yaml
Type: TopologyChart
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Topology.TopologyChart`

## OUTPUTS

- `ChartForgeX.Topology.TopologyLayoutDiagnosticReport`

## RELATED LINKS

- None
