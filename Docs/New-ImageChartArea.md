---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartArea
## SYNOPSIS
Creates area chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartArea [-Name] <string> [-Value] <double[]> [-Color <ChartColor>] [-SecondaryAxis] [<CommonParameters>]
```

## DESCRIPTION
Creates area chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartArea -Name 'Sales' -Value 10,20 -Color Green
```


### EXAMPLE 2
```powershell
New-ImageChart -XLabel Q1, Q2, Q3 -SecondaryYTitle Margin -FilePath area.png { New-ImageChartBar -Name Revenue -Value 12, 18, 25; New-ImageChartArea -Name Margin -Value 21, 24, 28 -SecondaryAxis }
```


## PARAMETERS

### -Color
Fill color.

```yaml
Type: ChartColor
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Label for the area.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Label
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SecondaryAxis
Plot the area against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Y values for the area.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ImagePlayground.ChartArea`: Area chart definition.

## RELATED LINKS

- None
