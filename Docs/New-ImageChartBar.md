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
New-ImageChartBar [-Name] <string> [-Value] <double[]> [-Color <ChartColor>] [-SecondaryAxis] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet inside New-ImageChart to define one bar series.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChartBar -Name 'Revenue' -Value 12,18,25 -Color Blue
```

Creates a single bar-series definition that can be passed into New-ImageChart.

### EXAMPLE 2
```powershell
PS> New-ImageChart -Title 'Builds' -XLabel Mon, Tue, Wed -FilePath builds.png {
    New-ImageChartBar -Name Passed -Value 41, 38, 45
    New-ImageChartLine -Name Target -Value 40, 40, 40 -Color '#c78404'
}
```

Bar and line definitions can be mixed in one chart.

## PARAMETERS

### -Color
Bar color.

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
Accept wildcard characters: False
```

### -SecondaryAxis
Plot the bars against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.

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
Values for the bar.

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

- `ImagePlayground.ChartBar`: Bar chart definition.

## RELATED LINKS

- None
