---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Resize-Image
## SYNOPSIS
Resizes an image object or a source file.

## SYNTAX
### FileDimensions (Default)
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [<CommonParameters>]
```

### FilePercentage
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> -Percentage <int> [<CommonParameters>]
```

### ObjectDimensions
```powershell
Resize-Image [-Image] <Image> [[-OutputPath] <string>] [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [<CommonParameters>]
```

### ObjectPercentage
```powershell
Resize-Image [-Image] <Image> [[-OutputPath] <string>] -Percentage <int> [<CommonParameters>]
```

## DESCRIPTION
Width and height bound the resized image while preserving its aspect ratio. Object input is updated and emitted for further edits or saving. Path input requires OutputPath and saves the result.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-Image in.png | Resize-Image -Width 1200 | Save-Image -FilePath out.png
```


### EXAMPLE 2
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100
```


### EXAMPLE 3
```powershell
$image | Resize-Image -Percentage 200
```


## PARAMETERS

### -DontRespectAspectRatio
Stretches to the supplied dimensions; an omitted dimension retains its original value.

```yaml
Type: SwitchParameter
Parameter Sets: FileDimensions, ObjectDimensions
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Source path, resolved in the current PowerShell filesystem location.

```yaml
Type: String
Parameter Sets: FileDimensions, FilePercentage
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Height
Requested height or maximum height when both bounds are supplied.

```yaml
Type: Int32
Parameter Sets: FileDimensions, ObjectDimensions
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Image
Editable object; the same instance is emitted after resizing.

```yaml
Type: Image
Parameter Sets: ObjectDimensions, ObjectPercentage
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -OutputPath
Destination path; required for path input and optional for object input.

```yaml
Type: String
Parameter Sets: FileDimensions, FilePercentage, ObjectDimensions, ObjectPercentage
Aliases: None
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Percentage
Positive percentage for uniform scaling relative to the original dimensions.

```yaml
Type: Int32
Parameter Sets: FilePercentage, ObjectPercentage
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Requested width or maximum width when both bounds are supplied.

The shared owner validates pixel and working-memory limits before allocation.

```yaml
Type: Int32
Parameter Sets: FileDimensions, ObjectDimensions
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
- `ImagePlayground.Image`

## OUTPUTS

- `ImagePlayground.Image`

## RELATED LINKS

- None
