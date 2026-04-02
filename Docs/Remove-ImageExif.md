---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Remove-ImageExif
## SYNOPSIS
Removes EXIF metadata from an image.

## SYNTAX
### Tag (Default)
```powershell
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -ExifTag <ExifTag[]> [<CommonParameters>]
```

### All
```powershell
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -All [<CommonParameters>]
```

## DESCRIPTION
Removes EXIF metadata from an image.

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-ImageExif -FilePath img.jpg -ExifTag ExifIFD.DateTimeOriginal
```

### EXAMPLE 2
```powershell
Remove-ImageExif -FilePath img.jpg -All
```

## PARAMETERS

### -All
Remove all tags.

```yaml
Type: SwitchParameter
Parameter Sets: All
Aliases: 
Possible values: 

Required: True
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -ExifTag
Tags to remove.

```yaml
Type: ExifTag[]
Parameter Sets: Tag
Aliases: 
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Path to the image file.

```yaml
Type: String
Parameter Sets: Tag, All
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FilePathOutput
Optional output path.

```yaml
Type: String
Parameter Sets: Tag, All
Aliases: 
Possible values: 

Required: False
Position: 1
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

