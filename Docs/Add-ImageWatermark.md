---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageWatermark
## SYNOPSIS
Adds a watermark image to another image.

## SYNTAX
### Placement (Default)
```powershell
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-Placement <WatermarkPlacement>] [-Opacity <float>] [-Padding <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]
```

### Coordinates
```powershell
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-X <int>] [-Y <int>] [-Opacity <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Adds a watermark image to another image.

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageWatermark -FilePath photo.png -OutputPath out.png -WatermarkPath logo.png -Placement BottomRight
```

## PARAMETERS

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Path to the source image.

```yaml
Type: String
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FlipMode
Flip mode for the image.

Possible values: None, Horizontal, Vertical

```yaml
Type: FlipMode
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: None, Horizontal, Vertical

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Opacity
Opacity of the watermark.

```yaml
Type: Single
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
Destination path for the watermarked image.

```yaml
Type: String
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Padding
Padding around the watermark.

```yaml
Type: Single
Parameter Sets: Placement
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 18
Accept pipeline input: False
Accept wildcard characters: True
```

### -Placement
Watermark placement when coordinates are not specified.

Possible values: TopLeft, TopRight, BottomLeft, BottomRight, Middle

```yaml
Type: WatermarkPlacement
Parameter Sets: Placement
Aliases: 
Possible values: TopLeft, TopRight, BottomLeft, BottomRight, Middle

Required: False
Position: named
Default value: Middle
Accept pipeline input: False
Accept wildcard characters: True
```

### -Rotate
Rotation angle in degrees.

```yaml
Type: Int32
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Spacing
Tile watermark across the image with given spacing.

```yaml
Type: Nullable`1
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -WatermarkPath
Image used as the watermark.

```yaml
Type: String
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -WatermarkPercentage
Scale of the watermark relative to the image.

```yaml
Type: Int32
Parameter Sets: Placement, Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X coordinate for custom placement.

```yaml
Type: Int32
Parameter Sets: Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
Y coordinate for custom placement.

```yaml
Type: Int32
Parameter Sets: Coordinates
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None

