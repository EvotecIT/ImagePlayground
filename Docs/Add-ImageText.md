---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Add-ImageText

## SYNOPSIS
Adds plain text or wrapped text to an image.

## SYNTAX

```
Add-ImageText [-FilePath] <String> [-OutputPath] <String> [-Text] <String> [-X] <Single> [-Y] <Single> [-Color <Color>] [-FontSize <Single>] [-FontFamily <String>] [-ShadowColor <Color>] [-ShadowOffsetX <Single>] [-ShadowOffsetY <Single>] [-OutlineColor <Color>] [-OutlineWidth <Single>] [<CommonParameters>]
```

## DESCRIPTION
`Add-ImageText` places text on an image at the given coordinates. Use `Add-ImageTextBox` to wrap the text inside a fixed width box.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-ImageText -FilePath .\input.png -OutputPath .\out.png -Text 'Sample' -X 10 -Y 10
```
Adds simple text in the top-left corner.

### Example 2
```powershell
PS C:\> Add-ImageTextBox -FilePath .\input.png -OutputPath .\out.png -Text 'Long text that will wrap' -X 10 -Y 10 -Width 150
```
Adds wrapped text constrained to a 150 pixel wide box.

### Example 3
```csharp
using SixLabors.ImageSharp;

ImageHelper.AddText(
    "input.png",
    "out.png",
    10,
    10,
    "Sample",
    Color.Black,
    fontSize: 24f);
```
Adds plain text using C#.

### Example 4
```csharp
using SixLabors.ImageSharp;

using var img = Image.Load("input.png");
img.AddText(10, 10, "Demo", Color.Red, 24);
img.AddTextBox(10, 40, "Wrapped demo text", 120, Color.Blue, 24);
img.Save("out2.png");
```
Adds both text and a wrapped text box using C#.

## PARAMETERS

### -FilePath
Path to the image that will receive the text.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Destination path where the modified image is saved.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
String to draw on the image.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -X
Horizontal position of the text in pixels.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: True
Position: 4
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -Y
Vertical position of the text in pixels.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: True
Position: 5
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -Color
Text color. Defaults to black.

```yaml
Type: Color
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FontSize
Size of the text in points. Defaults to 16.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -FontFamily
Name of the font family. Defaults to Arial.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShadowColor
Color for the drop shadow if one is desired.

```yaml
Type: Color
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShadowOffsetX
Horizontal offset of the shadow in pixels.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShadowOffsetY
Vertical offset of the shadow in pixels.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutlineColor
Color used to outline the text.

```yaml
Type: Color
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutlineWidth
Thickness of the text outline in pixels.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
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

