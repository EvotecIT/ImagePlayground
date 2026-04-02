---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartHistogram
## SYNOPSIS
New-ImageChartHistogram [-Name] <string> [-Values] <double[]> [-BinSize <int>] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartHistogram [-Name] <string> [-Values] <double[]> [-BinSize <int>] [<CommonParameters>]
```

## DESCRIPTION
New-ImageChartHistogram [-Name] <string> [-Values] <double[]> [-BinSize <int>] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartHistogram -Name 'Name'
```

## PARAMETERS

### -BinSize
{{ Fill BinSize Description }}

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

### -Name
{{ Fill Name Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Label
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Values
{{ Fill Values Description }}

```yaml
Type: Double[]
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

