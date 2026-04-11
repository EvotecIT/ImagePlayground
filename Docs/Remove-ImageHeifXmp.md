---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Remove-ImageHeifXmp
## SYNOPSIS
Removes the XMP metadata packet from a HEIF or HEIC file.

## SYNTAX
### __AllParameterSets
```powershell
Remove-ImageHeifXmp [-FilePath] <string> [[-FilePathOutput] <string>] [<CommonParameters>]
```

## DESCRIPTION
Clears the XMP packet from a HEIF or HEIC file by setting the existing XMP item extent length to zero.

This command does not decode HEIC image pixels and does not require native HEIF codecs. Updating requires an existing HEIF XMP item with a single writable file extent.

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-ImageHeifXmp -FilePath photo.heic
```

Clears the XMP packet from `photo.heic`.

### EXAMPLE 2
```powershell
Remove-ImageHeifXmp -FilePath photo.heic -FilePathOutput photo-clean.heic
```

Writes a copy of `photo.heic` with the XMP packet cleared to `photo-clean.heic`.

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

### -FilePathOutput
Optional output path. When omitted, the source file is overwritten.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- None

## RELATED LINKS

- None
