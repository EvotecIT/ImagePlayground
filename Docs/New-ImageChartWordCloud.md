---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartWordCloud
## SYNOPSIS
Creates word cloud chart term.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartWordCloud [-Text] <string> [-Weight] <double> [-Color <ChartColor>] [<CommonParameters>]
```

## DESCRIPTION
Creates word cloud chart term.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartWordCloud
```

## PARAMETERS

### -Color
Term color.

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

### -Text
Term text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Name
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Weight
Term weight.

```yaml
Type: Double
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

- `None`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None
