---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageExif
## SYNOPSIS
Sets an EXIF tag value in an image.

## SYNTAX
### __AllParameterSets
```powershell
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <ExifTag> [-Value] <Object> [<CommonParameters>]
```

## DESCRIPTION
The value must match the type declared by the selected EXIF tag, including ImageSharp wrapper types such as Number or Rational.

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageExif -FilePath img.jpg -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal) -Value (Get-Date)
```

## PARAMETERS

### -ExifTag
Tag to set.

```yaml
Type: ExifTag
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Image file to modify.

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

### -FilePathOutput
When not specified the source file is overwritten.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Value
Value for the tag.

```yaml
Type: Object
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
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

