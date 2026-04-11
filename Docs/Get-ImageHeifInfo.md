---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageHeifInfo
## SYNOPSIS
Gets HEIF container metadata without decoding image pixels.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageHeifInfo [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
Gets basic HEIF or HEIC container metadata such as brands, primary item information, item types, EXIF/XMP presence, item references, auxiliary image types, dimensions, rotation, mirror state, pixel aspect ratio spacing, pixel bit depths, and color information declared by HEIF item properties.

The returned object includes convenience properties such as `Width`, `Height`, `RotationDegrees`, `IsMirrored`, `PixelAspectRatioHorizontalSpacing`, `PixelAspectRatioVerticalSpacing`, `PixelBitDepths`, `ColorType`, `ColorPrimaries`, `TransferCharacteristics`, `MatrixCoefficients`, `FullRangeFlag`, `CodecConfigurationType`, `CodecConfigurationBytes`, `HasExif`, `HasXmp`, `PrimaryItem`, `ExifItem`, `XmpItem`, `Items`, and `References`.

Individual entries in `Items` can also include item-specific fields such as `ItemProtectionIndex`, `IsHidden`, `ContentEncoding`, `Location`, `PropertyAssociations`, `AuxiliaryType`, and `AuxiliarySubtypes` when those values are declared by the HEIF item metadata.

The `Location` property describes `iloc` storage metadata, including construction method, data reference index, resolved extents, `IsFileBacked`, `IsItemDataBoxBacked`, and `CanWriteSingleFileExtent` for the safe single-extent update case.

The `PropertyAssociations` property describes `ipma` metadata, including the one-based item property index, four-character property type, and essential flag.

Codec configuration metadata is exposed as raw property payload bytes for diagnostic use. This command does not parse or decode HEVC, AV1, or AVC bitstreams.

This command does not decode HEIC image pixels and does not require native HEIF codecs.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageHeifInfo -FilePath photo.heic
```

Returns the HEIF container metadata for `photo.heic`.

## PARAMETERS

### -FilePath
Path to the HEIF or HEIC file.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `ImagePlayground.HeifImageInfo`

## RELATED LINKS

- None
