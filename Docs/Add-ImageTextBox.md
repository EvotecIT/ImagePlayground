---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageTextBox
## SYNOPSIS
Add-ImageTextBox [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Width] <float> [[-Height] <float>] [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
Add-ImageTextBox [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Width] <float> [[-Height] <float>] [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]
```

## DESCRIPTION
Add-ImageTextBox [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Width] <float> [[-Height] <float>] [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageTextBox -FilePath 'C:\Path'
```

## PARAMETERS

### -Color
{{ Fill Color Description }}

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
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
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FontFamily
{{ Fill FontFamily Description }}

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

### -FontSize
{{ Fill FontSize Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -HorizontalAlignment
{{ Fill HorizontalAlignment Description }}

```yaml
Type: HorizontalAlignment
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Left, Right, Center

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutlineColor
{{ Fill OutlineColor Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutlineWidth
{{ Fill OutlineWidth Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
{{ Fill OutputPath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowColor
{{ Fill ShadowColor Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowOffsetX
{{ Fill ShadowOffsetX Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ShadowOffsetY
{{ Fill ShadowOffsetY Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Text
{{ Fill Text Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -VerticalAlignment
{{ Fill VerticalAlignment Description }}

```yaml
Type: VerticalAlignment
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Top, Center, Bottom

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
{{ Fill X Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
{{ Fill Y Description }}

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

