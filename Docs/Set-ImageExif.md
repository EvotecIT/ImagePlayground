---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Set-ImageExif

## SYNOPSIS
Sets EXIF tag to specific value

## SYNTAX

```
Set-ImageExif [-FilePath] <String> [[-FilePathOutput] <String>] [-ExifTag] <ExifTag> [-Value] <Object>
 [<CommonParameters>]
```

## DESCRIPTION
Sets EXIF tag to specific value

## EXAMPLES

### EXAMPLE 1
```
An example
```

## PARAMETERS

### -FilePath
File path to image to be processed for Exif Tag manipulation.
If FilePathOutput is not specified, the image will be overwritten.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePathOutput
File path to output image.
If not specified, the image will be overwritten.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExifTag
Exif Tag to be set

```yaml
Type: ExifTag
Parameter Sets: (All)
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Value to be set

```yaml
Type: Object
Parameter Sets: (All)
Aliases:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES
General notes

## RELATED LINKS
