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
Requires an existing HEIF XMP item with a single writable file extent.

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-ImageHeifXmp -FilePath photo.heic
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

### -FilePathOutput
When not specified the source file is overwritten.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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

- `System.Object`

## RELATED LINKS

- None
