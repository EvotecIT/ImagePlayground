---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageText
## SYNOPSIS
Adds text to an image at the provided coordinates and writes the updated
image to disk.

## SYNTAX
### __AllParameterSets
```powershell
Add-ImageText [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Color <OfficeColor>] [-FontSize <float>] [-FontFamily <string>] [-ShadowColor <OfficeColor>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <OfficeColor>] [-OutlineWidth <float>] [<CommonParameters>]
```

## DESCRIPTION
Adds text to an image at the provided coordinates and writes the updated
image to disk.

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageText -FilePath in.png -OutputPath out.png -Text "Sample" -X 10 -Y 10
```


### EXAMPLE 2
```powershell
$img = Get-Image -FilePath in.png
try {
    $img.AddText(10, 10, 'Sample', [OfficeIMO.Drawing.OfficeColor]::Black, 24)
    Save-Image -Image $img -FilePath out.png
} finally {
    $img.Dispose()
}
```


## PARAMETERS

### -Color
Text color.

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Source image path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -FontFamily
Font family.

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

### -FontSize
Font size.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutlineColor
Outline color.

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutlineWidth
Outline width.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Destination image path.

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

### -ShadowColor
Color of shadow.

```yaml
Type: OfficeColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShadowOffsetX
X offset for shadow.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShadowOffsetY
Y offset for shadow.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
Text to add.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -X
X coordinate.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Y
Y coordinate.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
