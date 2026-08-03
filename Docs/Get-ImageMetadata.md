---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageMetadata
## SYNOPSIS
Gets supported metadata profiles and provenance indicators from an image.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageMetadata [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
The result includes resolution, EXIF, XMP, ICC, IPTC, and lightweight C2PA provenance detection where supported.

C2PA presence does not by itself mean that an image was made with AI, and this command does not cryptographically validate C2PA claims.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageMetadata -FilePath image.png
```


### EXAMPLE 2
```powershell
$metadata = Get-ImageMetadata -FilePath image.png
$metadata.Provenance.HasC2paManifest
$metadata.Provenance.HasXmpAiDeclaration
```


## PARAMETERS

### -FilePath
Path to the image file.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `ImagePlayground.ImageMetadataInfo`

## RELATED LINKS

- None
