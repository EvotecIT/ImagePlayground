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
Set-ImageRotation [-FilePath] <string> [-OutputPath] <string> [-Degrees] <float> [-Async] [<CommonParameters>]
```

### Mode
```powershell
Set-ImageRotation [-FilePath] <string> [-OutputPath] <string> [-RotateMode] <RotateMode> [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use Degrees for arbitrary angles or for predefined rotations.

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

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: Degrees, Mode
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Degrees
Use for arbitrary rotations.

```yaml
Type: Single
Parameter Sets: Degrees
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image must exist.

```yaml
Type: String
Parameter Sets: Degrees, Mode
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: Degrees, Mode
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RotateMode
Use when rotating 90, 180 or 270 degrees.

Possible values: None, Rotate90, Rotate180, Rotate270

```yaml
Type: RotateMode
Parameter Sets: Mode
Aliases: 
Possible values: None, Rotate90, Rotate180, Rotate270

Required: True
Position: 2
Default value: None
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

