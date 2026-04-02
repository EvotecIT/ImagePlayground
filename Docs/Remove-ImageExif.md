---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Remove-ImageExif
## SYNOPSIS
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -ExifTag <ExifTag[]> [<CommonParameters>]

Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -All [<CommonParameters>]

## SYNTAX
### Tag (Default)
```powershell
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -ExifTag <ExifTag[]> [<CommonParameters>]
```

### All
```powershell
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -All [<CommonParameters>]
```

## DESCRIPTION
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -ExifTag <ExifTag[]> [<CommonParameters>]

Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -All [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-ImageExif -ExifTag @('Value')
```

### EXAMPLE 2
```powershell
Remove-ImageExif -All
```

## PARAMETERS

### -All
{{ Fill All Description }}

```yaml
Type: SwitchParameter
Parameter Sets: All
Aliases: None
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ExifTag
{{ Fill ExifTag Description }}

```yaml
Type: ExifTag[]
Parameter Sets: Tag
Aliases: None
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: Tag, All
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
Parameter Sets: Tag, All
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

