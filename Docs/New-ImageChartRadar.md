---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartRadar
## SYNOPSIS
Creates radar chart series data.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartRadar [-Name] <string> [-Category] <double[]> [-Value] <double[]> [-Color <ChartColor>] [<CommonParameters>]
```

## DESCRIPTION
Use at least three category coordinates. Multiple emitted definitions become multiple radar series in New-ImageChart.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChart -ChartsDefinition { New-ImageChartRadar -Name 'Current' -Category 1,2,3,4 -Value 82,68,91,74 -Color '#2563EB' } -FilePath radar.svg
```

Creates a four-axis radar chart using the shared ChartForgeX renderer.

## PARAMETERS

### -Category
Numeric category coordinates shared by the radar series.

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

### -Color
Series color.

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
Series name.

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

### -Value
Values for the radar series.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `ImagePlayground.ChartRadar`: Radar chart definition.

## RELATED LINKS

- None
