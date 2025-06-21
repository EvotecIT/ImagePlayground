---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageChartLine

## SYNOPSIS
Creates a line series definition for chart generation.

## SYNTAX

```
New-ImageChartLine [[-Name] <String>] [[-Value] <Array>] [-Color <Color>] [-Marker <MarkerShape>] [-Smooth] [<CommonParameters>]
```

## DESCRIPTION
Creates an object describing a single line series that can be passed to `New-ImageChart`.
The object includes optional color information and marker shape.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ImageChartLine -Name 'Series' -Value 1,2,3 -Marker FilledCircle
```

Creates a line series with circular markers.

## PARAMETERS

### -Name
Name displayed in chart legend.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Label

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Array of Y values for the line.

### -Color
Optional line color.

### -Marker
Shape of markers used on data points.

```yaml
Type: Array
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Smooth
Render the line using smooth curves.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
