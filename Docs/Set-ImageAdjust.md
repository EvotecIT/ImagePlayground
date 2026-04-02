---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageAdjust
## SYNOPSIS
Adjusts image properties.

## SYNTAX
### __AllParameterSets
```powershell
Set-ImageAdjust [-FilePath] <string> [-OutputPath] <string> [-Brightness <float>] [-Contrast <float>] [-Lightness <float>] [-Opacity <float>] [-Saturation <float>] [-Sepia <float>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Applies optional brightness, contrast, lightness, opacity, saturation or sepia adjustments.

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageAdjust -FilePath in.png -OutputPath out.png -Brightness 1.2 -Contrast 1.1
```

## PARAMETERS

### -Async
Use asynchronous processing.

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

### -Brightness
Brightness adjustment factor.

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

### -Contrast
Contrast adjustment factor.

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

### -FilePath
The image must exist.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Lightness
Lightness adjustment factor.

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

### -Opacity
Opacity adjustment factor.

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

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Saturation
Saturation adjustment factor.

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

### -Sepia
Sepia adjustment factor.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

