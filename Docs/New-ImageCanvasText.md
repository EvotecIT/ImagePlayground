---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageCanvasText
## SYNOPSIS
Creates a positioned text layer for a visual canvas.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageCanvasText [-X] <double> [-Y] <double> [-Width] <double> [-Text] <string> [-FontSize <double>] [-Color <ChartColor>] [-Alignment <TextAlignment>] [-Emphasized] [<CommonParameters>]
```

## DESCRIPTION
Creates a positioned text layer for a visual canvas.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageCanvasText -X 72 -Y 70 -Width 900 -Text 'Release 3.3' -FontSize 56 -Color White -Emphasized
```


## PARAMETERS

### -Alignment
Text alignment within the layer width.

```yaml
Type: TextAlignment
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Left, Center, Right

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Color
Text color.

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

### -Emphasized
Use the emphasized text treatment.

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

### -FontSize
Font size in canvas design pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
Text to render.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Text layout width.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -X
Horizontal position in canvas design pixels.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Y
Vertical position in canvas design pixels.

```yaml
Type: Double
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

- `ChartForgeX.Composition.VisualCanvasTextLayer`

## RELATED LINKS

- None
