---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageProvenance
## SYNOPSIS
Gets embedded C2PA containers and direct XMP generative-AI declarations from an image.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageProvenance [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
This command does not interpret the active C2PA manifest or cryptographically validate C2PA signatures, trust chains, or asset hashes.

A C2PA container records provenance but does not by itself mean that the image was made with AI. Use a conforming C2PA validator to inspect its active claim.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageProvenance -FilePath image.png
```


### EXAMPLE 2
```powershell
$info = Get-ImageProvenance -FilePath image.png
if ($info.HasXmpAiDeclaration) {
    Remove-ImageMetadata -FilePath image.png -OutputPath image-clean.png
}
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

- `None`

## RELATED LINKS

- None
