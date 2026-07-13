---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageHeifXmp
## SYNOPSIS
Gets the XMP metadata packet from a HEIF or HEIC file.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageHeifXmp [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
Returns the UTF-8 XMP packet when the container declares an XMP MIME metadata item.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageHeifXmp -FilePath photo.heic
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

- `System.String`

## RELATED LINKS

- None
