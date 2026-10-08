namespace ImagePlayground;

/// <summary>Font-aware text workflows delegated to the shared raster text engine.</summary>
public partial class Image {
    /// <summary>Measures authored text lines with the requested managed font family.</summary>
    public OfficeTextBlockLayout GetTextSize(string text, float fontSize, string fontFamilyName) => OfficeRasterText.Measure(text, fontSize, fontFamilyName);
    /// <summary>Draws text at a pixel location, with optional shadow and outline.</summary>
    public void AddText(float x, float y, string text, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f) => Apply(source => {
        var result = source.Clone(); var size = OfficeRasterText.Measure(text, fontSize, fontFamilyName);
        OfficeRasterText.Draw(result,text,x,y,Math.Max(1,size.Width),Math.Max(1,size.Height),color,fontSize,fontFamilyName,shadowColor:shadowColor,shadowOffsetX:shadowOffsetX,shadowOffsetY:shadowOffsetY,outlineColor:outlineColor,outlineWidth:outlineWidth);
        return result;
    });
    /// <summary>Wraps text to the supplied width without clipping its measured height.</summary>
    public void AddTextBox(float x, float y, string text, float boxWidth, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeTextAlignment horizontalAlignment = OfficeTextAlignment.Left, OfficeTextVerticalAlignment verticalAlignment = OfficeTextVerticalAlignment.Top, OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f) =>
        AddTextBox(x,y,text,boxWidth,0,color,fontSize,fontFamilyName,horizontalAlignment,verticalAlignment,shadowColor,shadowOffsetX,shadowOffsetY,outlineColor,outlineWidth);
    /// <summary>Draws wrapped text within a box, clipping when an explicit positive height is supplied.</summary>
    public void AddTextBox(float x, float y, string text, float boxWidth, float boxHeight, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeTextAlignment horizontalAlignment = OfficeTextAlignment.Left, OfficeTextVerticalAlignment verticalAlignment = OfficeTextVerticalAlignment.Top, OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f) => Apply(source => {
        if (boxWidth <= 0 || boxHeight < 0) {
            throw new ArgumentOutOfRangeException(nameof(boxWidth));
        }
        var result = source.Clone(); var measured = OfficeRasterText.Measure(text,fontSize,fontFamilyName,boxWidth);
        OfficeRasterText.Draw(result,text,x,y,boxWidth,boxHeight > 0 ? boxHeight : Math.Max(1,measured.Height),color,fontSize,fontFamilyName,horizontalAlignment,verticalAlignment,true,boxHeight>0,shadowColor,shadowOffsetX,shadowOffsetY,outlineColor,outlineWidth);
        return result;
    });
}