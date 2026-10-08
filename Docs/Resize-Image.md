---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Resize-Image
## SYNOPSIS
Resizes an image.

## SYNTAX
### HeightWidth (Default)
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [<CommonParameters>]
```

### Percentage
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Percentage <int>] [<CommonParameters>]
```

## DESCRIPTION
Width and height bound the resized image while preserving its aspect ratio. Use DontRespectAspectRatio to stretch to the supplied dimensions, or Percentage for uniform scaling.

## EXAMPLES

### EXAMPLE 1
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100
```


### EXAMPLE 2
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100 -DontRespectAspectRatio
```


### EXAMPLE 3
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Percentage 200
```


## PARAMETERS

### -DontRespectAspectRatio
Disables aspect ratio preservation. An omitted dimension retains its original value.

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
The image must exist.

```yaml
Type: String
Parameter Sets: HeightWidth, Percentage
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Height
When aspect ratio is preserved, a height alone determines the corresponding width.

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: HeightWidth, Percentage
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Percentage
Applies uniform scaling relative to the original size.

```yaml
Type: Int32
Parameter Sets: Percentage
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
When aspect ratio is preserved, a width alone determines the corresponding height.

```yaml
Type: Int32
Parameter Sets: HeightWidth
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

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
