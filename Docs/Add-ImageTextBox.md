---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Add-ImageTextBox

## SYNOPSIS
Adds wrapped text to an image within a defined box.

## SYNTAX
```powershell
Add-ImageTextBox [-FilePath] <String> [-OutputPath] <String> [-Text] <String> [-X] <Single> [-Y] <Single> [-Width] <Single> [-Height <Single>] [-Color <Color>] [-FontSize <Single>] [-FontFamily <String>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <Single>] [-ShadowOffsetY <Single>] [-OutlineColor <Color>] [-OutlineWidth <Single>] [<CommonParameters>]
```

## DESCRIPTION
Wraps the provided text to the specified width and draws it on the image. When `Height` is defined, the text is clipped to the given box height.

## EXAMPLES
### Example 1
```powershell
PS C:\> Add-ImageTextBox -FilePath .\input.png -OutputPath .\out.png -Text 'Long text that will wrap' -X 10 -Y 10 -Width 150
```
Draws the text in a 150 pixel wide box starting at (10,10).

### Example 2
```csharp
using SixLabors.ImageSharp;

ImageHelper.AddTextBox(
    "input.png",
    "out.png",
    10,
    10,
    "Long text that will wrap",
    150,
    0f,
    Color.Blue,
    fontSize: 20f,
    horizontalAlignment: SixLabors.Fonts.HorizontalAlignment.Center,
    verticalAlignment: SixLabors.Fonts.VerticalAlignment.Center);
```
Draws centered wrapped text using C#.

## PARAMETERS
See `Add-ImageText` for details on shared parameters.
