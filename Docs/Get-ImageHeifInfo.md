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
Returns brands, primary item information, item types, EXIF presence, and image dimensions when declared by HEIF item properties.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageHeifInfo -FilePath photo.heic
```


## PARAMETERS

### -FilePath
Path to the HEIF or HEIC file.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
