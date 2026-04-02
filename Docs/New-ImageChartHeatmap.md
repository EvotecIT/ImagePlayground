---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartHeatmap
## SYNOPSIS
Creates heatmap chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartHeatmap [-Name] <string> [-Matrix] <double[,]> [<CommonParameters>]
```

## DESCRIPTION
Creates heatmap chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartHeatmap -Name 'Matrix' -Matrix ((1,2),(3,4))
```

## PARAMETERS

### -Matrix
Matrix data for the heatmap.

```yaml
Type: Double[,]
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
Label for the heatmap.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `None`

## RELATED LINKS

- None

