---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Save-Image
## SYNOPSIS
Saves an image to disk or returns its encoded bytes as a stream.

## SYNTAX
### __AllParameterSets
```powershell
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <int>] [-CompressionLevel <int>] [-AsStream] [-Open] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to persist an Image instance after applying transformations.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Save-Image -Image $img
```

### EXAMPLE 2
```powershell
PS> Save-Image -Image $img -FilePath out.jpg -Quality 80
```

## PARAMETERS

### -AsStream
When used without FilePath, the cmdlet writes a stream object to the pipeline.

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
When omitted, the image is saved using the path already associated with the image object.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Image
Image object to save.

```yaml
Type: Image
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
Open file after saving.

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

