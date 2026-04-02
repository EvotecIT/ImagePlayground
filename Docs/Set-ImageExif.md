---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageExif
## SYNOPSIS
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <ExifTag> [-Value] <Object> [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <ExifTag> [-Value] <Object> [<CommonParameters>]
```

## DESCRIPTION
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <ExifTag> [-Value] <Object> [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageExif -FilePath 'C:\Path'
```

## PARAMETERS

### -ExifTag
{{ Fill ExifTag Description }}

```yaml
Type: ExifTag
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 2
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

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FilePathOutput
{{ Fill FilePathOutput Description }}

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

### -Value
{{ Fill Value Description }}

```yaml
Type: Object
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 3
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

