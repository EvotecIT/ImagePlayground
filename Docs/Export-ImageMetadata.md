---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Export-ImageMetadata
## SYNOPSIS
Exports metadata from an image.

## SYNTAX
### __AllParameterSets
```powershell
Export-ImageMetadata [-FilePath] <string> [[-OutputPath] <string>] [<CommonParameters>]
```

## DESCRIPTION
Exports metadata from an image.

## EXAMPLES

### EXAMPLE 1
```powershell
Export-ImageMetadata -FilePath in.jpg
```

### EXAMPLE 2
```powershell
Export-ImageMetadata -FilePath in.jpg -OutputPath meta.json
```

## PARAMETERS

### -FilePath
Source image file.

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

### -OutputPath
Optional path to write metadata JSON.

```yaml
Type: String
Parameter Sets: __AllParameterSets
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

- `System.String`

## RELATED LINKS

- None

