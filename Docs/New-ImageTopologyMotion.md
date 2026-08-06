---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageTopologyMotion
## SYNOPSIS
Creates script-free route motion options for topology output.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageTopologyMotion [-ScenarioId <string>] [-EdgeId <string[]>] [-DurationSeconds <double>] [-FramesPerSecond <double>] [-MaximumRasterFrames <int>] [-MarkerRadius <double>] [-MarkerColor <string>] [-NoLoop] [-NoEndpointPulses] [-Progress <double>] [<CommonParameters>]
```

## DESCRIPTION
Use a scenario id, explicit edge ids, or neither to animate the active or first scenario.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $motion = New-ImageTopologyMotion -ScenarioId request -DurationSeconds 4 -FramesPerSecond 12
```


## PARAMETERS

### -DurationSeconds
Animation duration in seconds.

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

### -EdgeId
Optional explicit edge identifiers used as the motion route.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FramesPerSecond
Frame rate sampled for GIF and APNG output.

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

### -MarkerColor
Optional marker color override.

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

### -MarkerRadius
Moving route marker radius.

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

### -MaximumRasterFrames
Maximum sampled raster frame count.

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

### -NoEndpointPulses
Disable endpoint node pulses for explicit-edge routes.

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

### -NoLoop
Render one animation cycle instead of looping.

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

### -Progress
Optional static progress position from zero to one.

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

### -ScenarioId
Optional scenario identifier used as the motion route.

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

- `ChartForgeX.Topology.TopologyMotionOptions`

## RELATED LINKS

- None
