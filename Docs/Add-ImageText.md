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

## PARAMETERS

### -FilePath
{{ Fill FilePath Description }}

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
{{ Fill OutputPath Description }}

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
{{ Fill Text Description }}

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
{{ Fill X Description }}

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
{{ Fill Y Description }}

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
{{ Fill Color Description }}

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
{{ Fill FontSize Description }}

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
{{ Fill FontFamily Description }}

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
{{ Fill ShadowColor Description }}

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
{{ Fill ShadowOffsetX Description }}

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
{{ Fill ShadowOffsetY Description }}

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
{{ Fill OutlineColor Description }}

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
{{ Fill OutlineWidth Description }}

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

