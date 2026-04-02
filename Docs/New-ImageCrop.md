---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageCrop
## SYNOPSIS
Creates a cropped version of an image using rectangular, circular or polygonal areas.

## SYNTAX
### Rectangle (Default)
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-X <int>] [-Y <int>] [-Width <int>] [-Height <int>] [-Open] [<CommonParameters>]
```

### Circle
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-CenterX <float>] [-CenterY <float>] [-Radius <float>] [-Open] [<CommonParameters>]
```

### Polygon
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-Points <PointF[]>] [-Open] [<CommonParameters>]
```

## DESCRIPTION
Creates a cropped version of an image using rectangular, circular or
polygonal areas.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageCrop -FilePath in.png -OutputPath out.png -X 10 -Y 10 -Width 100 -Height 100
```

### EXAMPLE 2
```powershell
New-ImageCrop -FilePath in.png -OutputPath out.png -CenterX 50 -CenterY 50 -Radius 25
```

## PARAMETERS

### -CenterX
X coordinate of the circle center.

```yaml
Type: Single
Parameter Sets: Circle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -CenterY
Y coordinate of the circle center.

```yaml
Type: Single
Parameter Sets: Circle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Path to the image being cropped.

```yaml
Type: String
Parameter Sets: Rectangle, Circle, Polygon
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
Height of the rectangle.

```yaml
Type: Int32
Parameter Sets: Rectangle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Open
Open the cropped file after creation.

```yaml
Type: SwitchParameter
Parameter Sets: Rectangle, Circle, Polygon
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
Where to save the cropped image.

```yaml
Type: String
Parameter Sets: Rectangle, Circle, Polygon
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Points
Points describing a polygon.

```yaml
Type: PointF[]
Parameter Sets: Polygon
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Radius
Radius of the circle.

```yaml
Type: Single
Parameter Sets: Circle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Width of the rectangle.

```yaml
Type: Int32
Parameter Sets: Rectangle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X coordinate for rectangle cropping.

```yaml
Type: Int32
Parameter Sets: Rectangle
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
Y coordinate for rectangle cropping.

```yaml
Type: Int32
Parameter Sets: Rectangle
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

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

