---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyScenarioStep
## SYNOPSIS
Creates one node or edge step for a topology scenario.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyScenarioStep [-Id] <string> [-Kind] <TopologyScenarioStepKind> [-Label <string>] [-Description <string>] [-DurationMilliseconds <int>] [<CommonParameters>]
```

## DESCRIPTION
Use the result inside New-ImageTopologyScenario to define an ordered route.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopologyScenarioStep -Id api-db -Kind Edge -Label 'Database call' -DurationMilliseconds 700
```


## PARAMETERS

### -Description
Optional step description.

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

### -DurationMilliseconds
Optional playback duration override in milliseconds.

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

### -Id
Referenced topology node or edge identifier.

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

### -Kind
Whether the step references a node or edge.

```yaml
Type: TopologyScenarioStepKind
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Node, Edge

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Label
Optional step label.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Topology.TopologyScenarioStep`

## RELATED LINKS

- None
