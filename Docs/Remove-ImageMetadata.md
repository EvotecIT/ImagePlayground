---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Remove-ImageMetadata
## SYNOPSIS
Removes selected metadata from an image.

## SYNTAX
### __AllParameterSets
```powershell
Remove-ImageMetadata [-FilePath] <string> [-OutputPath] <string> [-MetadataType <ImageMetadataType[]>] [-All] [-PassThru] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
JPEG and PNG files are rewritten without re-encoding their compressed image data. Choose individual metadata families, or use All for the compatibility behavior that removes every supported family. HEIF and HEIC cleanup is limited to EXIF and XMP.

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-ImageMetadata -FilePath in.jpg -OutputPath out.jpg -All
```


### EXAMPLE 2
```powershell
Remove-ImageMetadata -FilePath in.jpg -OutputPath out.jpg -MetadataType C2pa
```


## PARAMETERS

### -All
Remove every metadata family supported by the image format.

```yaml
Type: SwitchParameter
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
Source image file.

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

### -MetadataType
Metadata families to remove. When omitted, all supported metadata is removed for backward compatibility.

```yaml
Type: ImageMetadataType[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: None, Exif, Xmp, Iptc, Icc, C2pa, All

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

### -PassThru
Return a result describing which metadata was removed and whether re-encoding occurred.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
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
