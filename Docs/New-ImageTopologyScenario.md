---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyScenario
## SYNOPSIS
Creates an ordered topology scenario.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyScenario [-Id] <string> [-Label] <string> [-Step <TopologyScenarioStep[]>] [-StepDefinition <scriptblock>] [-Description <string>] [-Color <string>] [-StepDurationMilliseconds <int>] [-Loop] [-AutoPlay] [-Spotlight] [<CommonParameters>]
```

## DESCRIPTION
Scenarios drive HTML route controls, highlighted static views, and script-free SVG, GIF, and APNG route motion.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageTopologyScenario -Id request -Label 'Request flow' -StepDefinition { New-ImageTopologyScenarioStep -Id client-api -Kind Edge; New-ImageTopologyScenarioStep -Id api-db -Kind Edge } -AutoPlay
```


## PARAMETERS

### -AutoPlay
Allow interactive HTML output to begin playback automatically.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Color
Optional CSS color used to accent the scenario.

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

### -Description
Optional scenario description.

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
Stable scenario identifier.

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
Scenario label shown by HTML controls.

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

### -Loop
Loop scenario playback.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Spotlight
Dim topology elements that do not participate in the scenario.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Step
Scenario steps supplied directly.

```yaml
Type: TopologyScenarioStep[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StepDefinition
Script block that emits steps from New-ImageTopologyScenarioStep.

```yaml
Type: ScriptBlock
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -StepDurationMilliseconds
Default duration of each step in milliseconds.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Topology.TopologyScenario`

## RELATED LINKS

- None
