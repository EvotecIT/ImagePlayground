---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartBubble
## SYNOPSIS
Creates bubble chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartBubble [-Name] <string> [-X] <double[]> [-Y] <double[]> [-Size] <double[]> [-Color <Color>] [<CommonParameters>]
```

## DESCRIPTION
Creates bubble chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartBubble -Name 'Series1' -X 1,2,3 -Y 4,5,6 -Size 10,20,30 -Color Blue
```

## PARAMETERS

### -Color
Series color.

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
Label for the bubble series.

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

### -Size
Bubble sizes.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X values for the series.

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

### -Y
Y values for the series.

```yaml
Type: Double[]
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
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

