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
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <OfficeExifTag> [-Value] <Object> [<CommonParameters>]
```

## DESCRIPTION
The shared metadata API validates the selected tag and its value. Use OfficeRational values for unsigned EXIF fractions.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Set-ImageExif -FilePath img.jpg -ExifTag ([OfficeIMO.Drawing.OfficeExifTag]::DateTimeOriginal) -Value (Get-Date -Format 'yyyy:MM:dd HH:mm:ss')
```


## PARAMETERS

### -ExifTag
Tag to set.

```yaml
Type: OfficeExifTag
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Image file to modify.

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

### -FilePathOutput
When not specified the source file is overwritten.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Value for the tag.

```yaml
Type: Object
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
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
