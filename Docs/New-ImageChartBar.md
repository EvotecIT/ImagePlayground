---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartBar
## SYNOPSIS
Creates bar chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartBar [-Name] <string> [-Value] <double[]> [-Color <Color>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageChart to define one bar series.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartBar -Name 'Revenue' -Value 12,18,25 -Color Blue
```

Creates a single bar-series definition that can be passed into New-ImageChart.

## PARAMETERS

### -Color
Bar color.

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

### -Name
Label for the bar data.

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

### -Value
Values for the bar.

```yaml
Type: Double[]
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

