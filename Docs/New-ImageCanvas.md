---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageCanvas
## SYNOPSIS
Creates a fixed-size visual canvas for social images, wallpapers, report covers, and announcement cards.

## SYNTAX
### Definition (Default)
```powershell
New-ImageCanvas [-LayerDefinition] <scriptblock> -FilePath <string> [-Preset <ImageCanvasPreset>] [-Width <int>] [-Height <int>] [-Title <string>] [-BackgroundTop <ChartColor>] [-BackgroundBottom <ChartColor>] [-Backdrop <VisualCanvasBackdropStyle>] [-PngOutputScale <int>] [-Show] [-PassThru] [<CommonParameters>]
```

### Layer
```powershell
New-ImageCanvas -Layer <VisualCanvasLayer[]> -FilePath <string> [-Preset <ImageCanvasPreset>] [-Width <int>] [-Height <int>] [-Title <string>] [-BackgroundTop <ChartColor>] [-BackgroundBottom <ChartColor>] [-Backdrop <VisualCanvasBackdropStyle>] [-PngOutputScale <int>] [-Show] [-PassThru] [<CommonParameters>]
```

### Canvas
```powershell
New-ImageCanvas -Canvas <VisualCanvas> -FilePath <string> [-Title <string>] [-BackgroundTop <ChartColor>] [-BackgroundBottom <ChartColor>] [-Backdrop <VisualCanvasBackdropStyle>] [-PngOutputScale <int>] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Layer commands emit native ChartForgeX layers; ImagePlayground only binds PowerShell inputs and selects the output format.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageCanvas -Preset SocialPreview -Title 'ChartForgeX 1.3' -Backdrop TechHorizon -LayerDefinition { New-ImageCanvasText -X 72 -Y 72 -Width 1000 -Text 'ChartForgeX 1.3' -FontSize 58 -Color White -Emphasized; New-ImageCanvasInfoTile -X 72 -Y 190 -Width 360 -Height 150 -Icon SVG -Label 'Renderer' -Value 'Dependency-free' } -FilePath preview.png
```


## PARAMETERS

### -Backdrop
Built-in canvas backdrop.

```yaml
Type: VisualCanvasBackdropStyle
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values: Transparent, Plain, TechHorizon

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackgroundBottom
Bottom background color for a vertical gradient.

```yaml
Type: ChartColor
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackgroundTop
Top or solid background color.

```yaml
Type: ChartColor
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Canvas
Existing ChartForgeX visual canvas to render.

```yaml
Type: VisualCanvas
Parameter Sets: Canvas
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -FilePath
Output image path.

```yaml
Type: String
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Custom canvas height.

```yaml
Type: Int32
Parameter Sets: Definition, Layer
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Layer
Canvas layers supplied directly or through the pipeline.

```yaml
Type: VisualCanvasLayer[]
Parameter Sets: Layer
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -LayerDefinition
Script block that emits visual canvas layers.

```yaml
Type: ScriptBlock
Parameter Sets: Definition
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the configured ChartForgeX canvas to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PngOutputScale
PNG output pixel multiplier.

```yaml
Type: Int32
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Preset
Built-in canvas size preset.

```yaml
Type: ImageCanvasPreset
Parameter Sets: Definition, Layer
Aliases: None
Possible values: Custom, SocialPreview, DesktopWallpaper

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the generated canvas.

```yaml
Type: SwitchParameter
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Accessibility title for SVG output.

```yaml
Type: String
Parameter Sets: Definition, Layer, Canvas
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Custom canvas width.

```yaml
Type: Int32
Parameter Sets: Definition, Layer
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Composition.VisualCanvasLayer[]`
- `ChartForgeX.Composition.VisualCanvas`

## OUTPUTS

- `ChartForgeX.Composition.VisualCanvas`

## RELATED LINKS

- None
