---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Save-Image
## SYNOPSIS
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <int>] [-CompressionLevel <int>] [-AsStream] [-Open] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <int>] [-CompressionLevel <int>] [-AsStream] [-Open] [<CommonParameters>]
```

## DESCRIPTION
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <int>] [-CompressionLevel <int>] [-AsStream] [-Open] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Save-Image -FilePath 'C:\Path'
```

## PARAMETERS

### -AsStream
{{ Fill AsStream Description }}

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -CompressionLevel
{{ Fill CompressionLevel Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Image
{{ Fill Image Description }}

```yaml
Type: Image
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Open
{{ Fill Open Description }}

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Quality
{{ Fill Quality Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
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

- `System.Object`

## RELATED LINKS

- None

