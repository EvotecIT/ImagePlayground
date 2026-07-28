---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageVisualMotionCue
## SYNOPSIS
Creates one named motion cue for an animated ChartForgeX visual story.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageVisualMotionCue [-TargetId] <string> [-Effect] <VisualMotionEffect> [-DelaySeconds <double>] [-DurationSeconds <double>] [-Easing <VisualMotionEasing>] [-DistancePixels <double>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside the MotionDefinition block of New-ImageVisualStory. Each cue targets the stable id assigned to a visual-grid panel or the built-in title and subtitle targets.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.9
```

Creates a restrained left-to-right title reveal with the default emphasized easing.

## PARAMETERS

### -DelaySeconds
Delay before the cue starts, in seconds.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -DistancePixels
Travel distance used by positional effects, in pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 12
Accept pipeline input: False
Accept wildcard characters: True
```

### -DurationSeconds
Cue duration, in seconds.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 0,7
Accept pipeline input: False
Accept wildcard characters: True
```

### -Easing
Timing curve used by the cue.

Possible values: Linear, EaseOut, EaseInOut, Emphasized

```yaml
Type: VisualMotionEasing
Parameter Sets: __AllParameterSets
Aliases:
Possible values: Linear, EaseOut, EaseInOut, Emphasized

Required: False
Position: named
Default value: Emphasized
Accept pipeline input: False
Accept wildcard characters: True
```

### -Effect
Entrance or emphasis effect.

Possible values: Fade, Rise, Reveal, Scale, Pulse

```yaml
Type: VisualMotionEffect
Parameter Sets: __AllParameterSets
Aliases:
Possible values: Fade, Rise, Reveal, Scale, Pulse

Required: True
Position: 1
Default value: Fade
Accept pipeline input: False
Accept wildcard characters: True
```

### -TargetId
Stable panel id, or the built-in title or subtitle target.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.Motion.VisualMotionCue`

## RELATED LINKS

- None
