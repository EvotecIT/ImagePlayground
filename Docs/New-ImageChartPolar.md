---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartPolar
## SYNOPSIS
Creates polar chart data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartPolar [-Name] <string> [-Angle] <double[]> [-Value] <double[]> [-Color <Color>] [<CommonParameters>]
```

## DESCRIPTION
Creates polar chart data item.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChartPolar -Name 'Series1' -Angle 0,1.57 -Value 1,2 -Color Blue
```

### EXAMPLE 2
```powershell
New-ImageChartPolar -Name 'Advanced' -Angle 0,1.57,3.14 -Value 1,2,1 -Color Red
```

## PARAMETERS

### -Angle
Angle values for the series.

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
Label for the series.

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
Radius values for the series.

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

