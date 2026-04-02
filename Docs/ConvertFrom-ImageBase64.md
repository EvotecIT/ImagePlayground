---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# ConvertFrom-ImageBase64
## SYNOPSIS
Converts a Base64 encoded string into an image file.

## SYNTAX
### __AllParameterSets
```powershell
ConvertFrom-ImageBase64 [-Base64] <string> [-OutputPath] <string> [-Open] [<CommonParameters>]
```

## DESCRIPTION
Converts a Base64 encoded string into an image file.

## EXAMPLES

### EXAMPLE 1
```powershell
ConvertFrom-ImageBase64 -Base64 $content -OutputPath out.png
```

### EXAMPLE 2
```powershell
ConvertFrom-ImageBase64 -Base64 $content -OutputPath out.png -Open
```

## PARAMETERS

### -Base64
Base64 encoded image data.

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

### -Open
Open the newly created file after saving.

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

### -OutputPath
Path where the image will be written.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None

