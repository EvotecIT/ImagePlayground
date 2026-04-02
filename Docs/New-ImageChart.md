---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChart
## SYNOPSIS
New-ImageChart [[-ChartsDefinition] <scriptblock>] -FilePath <string> [-Definition <Charts+ChartDefinition[]>] [-AnnotationsDefinition <scriptblock>] [-Annotation <Charts+ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageChart [[-ChartsDefinition] <scriptblock>] -FilePath <string> [-Definition <Charts+ChartDefinition[]>] [-AnnotationsDefinition <scriptblock>] [-Annotation <Charts+ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [<CommonParameters>]
```

## DESCRIPTION
New-ImageChart [[-ChartsDefinition] <scriptblock>] -FilePath <string> [-Definition <Charts+ChartDefinition[]>] [-AnnotationsDefinition <scriptblock>] [-Annotation <Charts+ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageChart -FilePath 'C:\Path'
```

## PARAMETERS

### -Annotation
{{ Fill Annotation Description }}

```yaml
Type: ChartAnnotation[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -AnnotationsDefinition
{{ Fill AnnotationsDefinition Description }}

```yaml
Type: ScriptBlock
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ChartsDefinition
{{ Fill ChartsDefinition Description }}

```yaml
Type: ScriptBlock
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Definition
{{ Fill Definition Description }}

```yaml
Type: ChartDefinition[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
{{ Fill Show Description }}

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShowGrid
{{ Fill ShowGrid Description }}

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Theme
{{ Fill Theme Description }}

```yaml
Type: ChartTheme
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Default, Dark, Light

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -XTitle
{{ Fill XTitle Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -YTitle
{{ Fill YTitle Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ImagePlayground.Charts+ChartDefinition[]
System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

