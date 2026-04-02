---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# ConvertTo-Image
## SYNOPSIS
Converts an image to a different format.

## SYNTAX
### __AllParameterSets
```powershell
ConvertTo-Image [-FilePath] <string> [-OutputPath] <string> [-Quality <int>] [-CompressionLevel <int>] [<CommonParameters>]
```

## DESCRIPTION
Outputs a new file using the extension from OutputPath.

When OutputPath ends with .ico, the file is only copied if the source file is also an icon.

## EXAMPLES

### EXAMPLE 1
```powershell
ConvertTo-Image -FilePath image.png -OutputPath image.jpg -Quality 85
```

### EXAMPLE 2
```powershell
ConvertTo-Image -FilePath photo.jpg -OutputPath photo.png -CompressionLevel 6
```

## PARAMETERS

### -CompressionLevel
Compression level for PNG images.

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

### -FilePath
The file must exist.

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
The extension determines the output format.

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

### -Quality
Quality for JPEG or WEBP images.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

