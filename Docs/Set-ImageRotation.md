---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageRotation
## SYNOPSIS
Sets image rotation.

## SYNTAX
### Degrees (Default)
```powershell
Set-ImageRotation [-FilePath] <string> [-OutputPath] <string> [-Degrees] <float> [<CommonParameters>]
```

### Mode
```powershell
Set-ImageRotation [-FilePath] <string> [-OutputPath] <string> [-RotateMode] <RotateMode> [<CommonParameters>]
```

## DESCRIPTION
Use Degrees for arbitrary angles or RotateMode for predefined rotations.

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageRotation -FilePath in.png -OutputPath out.png -Degrees 90
```


### EXAMPLE 2
```powershell
Set-ImageRotation -FilePath in.png -OutputPath out.png -RotateMode Rotate180
```


## PARAMETERS

### -Degrees
Use for arbitrary rotations.

```yaml
Type: Single
Parameter Sets: Degrees
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
The image must exist.

```yaml
Type: String
Parameter Sets: Degrees, Mode
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: Degrees, Mode
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -RotateMode
Use when rotating 90, 180 or 270 degrees.

```yaml
Type: RotateMode
Parameter Sets: Mode
Aliases: None
Possible values: None, Rotate90, Rotate180, Rotate270

Required: True
Position: 2
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
