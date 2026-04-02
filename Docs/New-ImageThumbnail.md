---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageThumbnail
## SYNOPSIS
Creates thumbnails for all images in a directory.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageThumbnail [-DirectoryPath] <string> [-OutputDirectory] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]
```

## DESCRIPTION
Creates thumbnails for all images in a directory.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageThumbnail -DirectoryPath images -OutputDirectory thumbs -Width 64 -Height 64
```

## PARAMETERS

### -DirectoryPath
Directory containing images.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -DontRespectAspectRatio
Ignore aspect ratio.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
Thumbnail height.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 100
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputDirectory
Destination directory for thumbnails.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Sampler
Resampling algorithm.

Possible values: NearestNeighbor, Box, Triangle, Hermite, Lanczos2, Lanczos3, Lanczos5, Lanczos8, MitchellNetravali, CatmullRom, Robidoux, RobidouxSharp, Spline, Welch

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Thumbnail width.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 100
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None

