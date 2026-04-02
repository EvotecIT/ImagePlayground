---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# ConvertTo-ImageBase64
## SYNOPSIS
Converts an image file into a Base64 encoded string.

## SYNTAX
### __AllParameterSets
```powershell
ConvertTo-ImageBase64 [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
Converts an image file into a Base64 encoded string.

## EXAMPLES

### EXAMPLE 1
```powershell
ConvertTo-ImageBase64 -FilePath in.png
```

### EXAMPLE 2
```powershell
$b64 = ConvertTo-ImageBase64 -FilePath photo.jpg; Invoke-RestMethod -Uri https://example/upload -Body $b64
```

## PARAMETERS

### -FilePath
Path to the image to convert.

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

- `System.String`

## RELATED LINKS

- None

