---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Add-ImageWatermark
## SYNOPSIS
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-Placement <WatermarkPlacement>] [-Opacity <float>] [-Padding <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-X <int>] [-Y <int>] [-Opacity <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

## SYNTAX
### Placement (Default)
```powershell
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-Placement <WatermarkPlacement>] [-Opacity <float>] [-Padding <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]
```

### Coordinates
```powershell
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-X <int>] [-Y <int>] [-Opacity <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-Placement <WatermarkPlacement>] [-Opacity <float>] [-Padding <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-X <int>] [-Y <int>] [-Opacity <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Add-ImageWatermark -FilePath 'C:\Path'
```

## PARAMETERS

### -Async
{{ Fill Async Description }}

```yaml
Type: SwitchParameter
Parameter Sets: Placement, Coordinates
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
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FlipMode
{{ Fill FlipMode Description }}

```yaml
Type: FlipMode
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: None, Horizontal, Vertical

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Opacity
{{ Fill Opacity Description }}

```yaml
Type: Single
Parameter Sets: Placement, Coordinates
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
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Padding
{{ Fill Padding Description }}

```yaml
Type: Single
Parameter Sets: Placement
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Placement
{{ Fill Placement Description }}

```yaml
Type: WatermarkPlacement
Parameter Sets: Placement
Aliases: None
Possible values: TopLeft, TopRight, BottomLeft, BottomRight, Middle

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Rotate
{{ Fill Rotate Description }}

```yaml
Type: Int32
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Spacing
{{ Fill Spacing Description }}

```yaml
Type: Nullable`1
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -WatermarkPath
{{ Fill WatermarkPath Description }}

```yaml
Type: String
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -WatermarkPercentage
{{ Fill WatermarkPercentage Description }}

```yaml
Type: Int32
Parameter Sets: Placement, Coordinates
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
{{ Fill X Description }}

```yaml
Type: Int32
Parameter Sets: Coordinates
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
{{ Fill Y Description }}

```yaml
Type: Int32
Parameter Sets: Coordinates
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

- `None`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

