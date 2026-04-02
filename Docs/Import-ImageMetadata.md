---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Import-ImageMetadata
## SYNOPSIS
Imports metadata into an image.

## SYNTAX
### __AllParameterSets
```powershell
Import-ImageMetadata [-FilePath] <string> [-MetadataPath] <string> [[-OutputPath] <string>] [<CommonParameters>]
```

## DESCRIPTION
Imports metadata into an image.

## EXAMPLES

### EXAMPLE 1
```powershell
Import-ImageMetadata -FilePath img.jpg -MetadataPath meta.json
```

### EXAMPLE 2
```powershell
Import-ImageMetadata -FilePath img.jpg -MetadataPath meta.json -OutputPath out.jpg
```

## PARAMETERS

### -FilePath
Image to update.

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

### -MetadataPath
JSON metadata file.

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

### -OutputPath
Destination image path. Defaults to FilePath.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 2
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

