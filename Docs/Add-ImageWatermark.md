---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Add-ImageWatermark

## SYNOPSIS
Adds an image watermark to another image.

## SYNTAX

### Placement (Default)
```
Add-ImageWatermark [-FilePath] <String> [-OutputPath] <String> [-WatermarkPath] <String> [-Placement <WatermarkPlacement>] [-Opacity <Single>] [-Padding <Single>] [-Rotate <Int32>] [-FlipMode <FlipMode>] [-WatermarkPercentage <Int32>] [<CommonParameters>]
```

### Coordinates
```
Add-ImageWatermark [-FilePath] <String> [-OutputPath] <String> [-WatermarkPath] <String> [-X <Int32>] [-Y <Int32>] [-Opacity <Single>] [-Rotate <Int32>] [-FlipMode <FlipMode>] [-WatermarkPercentage <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Places a watermark image on a picture either by predefined placement or by coordinates. Optionally adjusts opacity, rotation and size.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-ImageWatermark -FilePath .\input.jpg -OutputPath .\watermark.png -WatermarkPath .\logo.png
```
Adds a watermark using default placement.

## PARAMETERS

### -FilePath
Path to the source image.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Path where the resulting image should be saved.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WatermarkPath
Path to the image used as the watermark.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Placement
Watermark placement relative to the original image when coordinates are not used.

```yaml
Type: WatermarkPlacement
Parameter Sets: Placement
Aliases:

Required: False
Position: Named
Default value: Middle
Accept pipeline input: False
Accept wildcard characters: False
```

### -X
Horizontal coordinate for the watermark.

```yaml
Type: Int32
Parameter Sets: Coordinates
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -Y
Vertical coordinate for the watermark.

```yaml
Type: Int32
Parameter Sets: Coordinates
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -Opacity
Opacity level of the watermark from 0 to 1.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -Padding
Padding used when placement mode is selected.

```yaml
Type: Single
Parameter Sets: Placement
Aliases:

Required: False
Position: Named
Default value: 18
Accept pipeline input: False
Accept wildcard characters: False
```

### -Rotate
Rotation of the watermark in degrees.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -FlipMode
Specifies whether the watermark should be flipped.

```yaml
Type: FlipMode
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WatermarkPercentage
Percentage size of the watermark relative to the base image width.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
