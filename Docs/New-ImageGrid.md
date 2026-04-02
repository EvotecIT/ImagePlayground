---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageGrid
## SYNOPSIS
New-ImageGrid [-FilePath] <string> [-Width] <int> [-Height] <int> [-Color <Color>] [-Open] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageGrid [-FilePath] <string> [-Width] <int> [-Height] <int> [-Color <Color>] [-Open] [<CommonParameters>]
```

## DESCRIPTION
New-ImageGrid [-FilePath] <string> [-Width] <int> [-Height] <int> [-Color <Color>] [-Open] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageGrid -FilePath 'C:\Path'
```

## PARAMETERS

### -Color
{{ Fill Color Description }}

```yaml
Type: Color
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

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 2
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

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
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

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

