---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageTextBox
## SYNOPSIS
Adds wrapped text to an image within a box.

## SYNTAX
### __AllParameterSets
```powershell
Add-ImageTextBox [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Width] <float> [[-Height] <float>] [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]
```

## DESCRIPTION
Adds wrapped text to an image within a box.

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageTextBox -FilePath in.png -OutputPath out.png -Text "Sample text" -X 10 -Y 10 -Width 100
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

### -Height
Height of the text box.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 6
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -HorizontalAlignment
Horizontal alignment.

Possible values: Left, Right, Center

```yaml
Type: HorizontalAlignment
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: Left, Right, Center

Required: False
Position: named
Default value: Left
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

### -VerticalAlignment
Vertical alignment.

Possible values: Top, Center, Bottom

```yaml
Type: VerticalAlignment
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: Top, Center, Bottom

Required: False
Position: named
Default value: Top
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Width of the text box.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 5
Default value: 0
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

