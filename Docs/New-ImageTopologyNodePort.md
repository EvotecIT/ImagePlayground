---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyNodePort
## SYNOPSIS
Creates a named attachment port for a topology node.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyNodePort [-Id] <string> [-Side] <TopologyEdgePort> [-Offset <double>] [-Label <string>] [<CommonParameters>]
```

## DESCRIPTION
Creates a named attachment port for a topology node.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageTopologyNodePort
```

## PARAMETERS

### -Id
Node-local stable port identifier.

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

### -Label
Optional visible or host-facing label.

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

### -Offset
Normalized position along the side from zero to one.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Side
Explicit node boundary side.

```yaml
Type: TopologyEdgePort
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Auto, Top, Right, Bottom, Left

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Topology.TopologyNodePort`

## RELATED LINKS

- None
