---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Save-Image
## SYNOPSIS
Saves an image object to disk or returns its encoded bytes as a stream.

## SYNTAX
### File (Default)
```powershell
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <Int32>] [-CompressionLevel <Int32>] [-EncodingOptions <OfficeRasterEncodingOptions>] [-Open] [<CommonParameters>]
```

### Stream
```powershell
Save-Image [-Image] <Image> -AsStream [-Quality <Int32>] [-CompressionLevel <Int32>] [-EncodingOptions <OfficeRasterEncodingOptions>] [-Format <ImageType>] [<CommonParameters>]
```

## DESCRIPTION
Accepts editable image objects from Get-Image and Resize-Image.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Get-Image in.png | Resize-Image -Width 1200 | Save-Image -FilePath out.png
```


### EXAMPLE 2
```powershell
PS> Get-Image in.jpg | Save-Image -AsStream -Format Png
```


## PARAMETERS

### -AsStream
Returns an encoded stream at position zero; the caller owns the stream.

```yaml
Type: SwitchParameter
Parameter Sets: Stream
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CompressionLevel
Compression level for PNG images.

```yaml
Type: Int32
Parameter Sets: File, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EncodingOptions
Owned encoder settings; simple quality and compression controls override their matching settings.

```yaml
Type: OfficeRasterEncodingOptions
Parameter Sets: File, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Optional destination path; omitted uses the path associated with the image.

```yaml
Type: String
Parameter Sets: File
Aliases: None
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Format
Explicit stream output format; omitted uses the image's detected supported default.

```yaml
Type: ImageType
Parameter Sets: Stream
Aliases: None
Possible values: Bmp, Gif, Jpeg, Pbm, Png, Tga, Tiff, WebP, Icon

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Image
Editable image object to save.

```yaml
Type: Image
Parameter Sets: File, Stream
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Open
Opens the completed file.

```yaml
Type: SwitchParameter
Parameter Sets: File
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Quality
Quality for JPEG or WEBP images.

```yaml
Type: Int32
Parameter Sets: File, Stream
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

- `ImagePlayground.Image`

## OUTPUTS

- `None`

## RELATED LINKS

- None
