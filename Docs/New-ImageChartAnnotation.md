---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChartAnnotation
## SYNOPSIS
Creates chart annotation data item.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChartAnnotation [-X] <double> [-Y] <double> [-Text] <string> [-Arrow] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet with New-ImageChart to highlight notable points on a generated chart.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChartAnnotation -X 3 -Y 61 -Text 'Peak usage' -Arrow
```

Creates an annotation definition that can be passed to New-ImageChart via -Annotation or -AnnotationsDefinition.

## PARAMETERS

### -Arrow
Display arrow.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Text
Annotation text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
X coordinate for annotation.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
Y coordinate for annotation.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: 0
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

