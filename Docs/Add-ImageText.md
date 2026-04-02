---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageText
## SYNOPSIS
Adds text to an image at the provided coordinates and writes the updated image to disk.

## SYNTAX
### __AllParameterSets
```powershell
Add-ImageText [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]
```

## DESCRIPTION
Adds text to an image at the provided coordinates and writes the updated image to disk.

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageText -FilePath in.png -OutputPath out.png -Text "Sample" -X 10 -Y 10
```

### EXAMPLE 2
```powershell
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using var img = Image.Load("in.png");
img.AddText(10, 10, "Sample", Color.Black, 24);
img.Save("out.png");
```

## PARAMETERS

### -Color
Text color.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Source image path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FontFamily
Font family.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: Arial
Accept pipeline input: False
Accept wildcard characters: True
```

### -FontSize
Font size.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 16
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutlineColor
Outline color.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutlineWidth
Outline width.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
Destination image path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowColor
Color of shadow.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowOffsetX
X offset for shadow.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowOffsetY
Y offset for shadow.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Text
Text to add.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X coordinate.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
Y coordinate.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 4
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

