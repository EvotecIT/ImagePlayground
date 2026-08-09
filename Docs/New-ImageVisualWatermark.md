---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageVisualWatermark
## SYNOPSIS
Creates a deterministic text or image watermark for ChartForgeX visual artifacts.

## SYNTAX
### Text (Default)
```powershell
New-ImageVisualWatermark [-Text] <string> [-Anchor <VisualCanvasAnchor>] [-OffsetX <double>] [-OffsetY <double>] [-Padding <double>] [-Opacity <double>] [-RotationDegrees <double>] [-Scale <double>] [-Color <ChartColor>] [-FontSize <double>] [-Repeat] [-RepeatSpacingX <double>] [-RepeatSpacingY <double>] [<CommonParameters>]
```

### Image
```powershell
New-ImageVisualWatermark [-ImagePath] <string> [-Anchor <VisualCanvasAnchor>] [-OffsetX <double>] [-OffsetY <double>] [-Padding <double>] [-Opacity <double>] [-RotationDegrees <double>] [-Scale <double>] [-Repeat] [-RepeatSpacingX <double>] [-RepeatSpacingY <double>] [<CommonParameters>]
```

## DESCRIPTION
Creates a deterministic text or image watermark for ChartForgeX visual artifacts.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $mark = New-ImageVisualWatermark -Text CONFIDENTIAL -Anchor Center -RotationDegrees -28 -Opacity 0.15
```

The watermark can be passed to Export-ImageVisualArtifact or New-ImageTopology.

## PARAMETERS

### -Anchor
Watermark anchor on the artifact canvas.

```yaml
Type: VisualCanvasAnchor
Parameter Sets: Text, Image
Aliases: None
Possible values: TopLeft, TopCenter, TopRight, MiddleLeft, Center, MiddleRight, BottomLeft, BottomCenter, BottomRight

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Color
Text color.

```yaml
Type: ChartColor
Parameter Sets: Text
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FontSize
Base text size in pixels.

```yaml
Type: Double
Parameter Sets: Text
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImagePath
Raster watermark image path.

```yaml
Type: String
Parameter Sets: Image
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OffsetX
Horizontal offset from the selected anchor.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OffsetY
Vertical offset from the selected anchor.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Opacity
Watermark opacity from zero to one.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Padding
Inset from anchored canvas edges.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Repeat
Render repeated watermark tiles across the canvas.

```yaml
Type: SwitchParameter
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RepeatSpacingX
Horizontal spacing between repeated watermark anchors.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RepeatSpacingY
Vertical spacing between repeated watermark anchors.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RotationDegrees
Clockwise watermark rotation in degrees.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scale
Positive watermark scale.

```yaml
Type: Double
Parameter Sets: Text, Image
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
Watermark text.

```yaml
Type: String
Parameter Sets: Text
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ChartForgeX.VisualArtifacts.VisualWatermark`

## RELATED LINKS

- None
