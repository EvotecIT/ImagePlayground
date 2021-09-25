function New-ImageChart {
    [cmdletBinding()]
    param(
        [Parameter(Position = 0,Mandatory)][scriptblock] $ChartEntries,
        [string] $FilePath
    )


    $Bitmap = [SkiaSharp.SKBitmap]::new(500, 500)
    $Bitmap.Erase([SkiaSharp.SKColor]::Empty)
    $Canvas = [SkiaSharp.SKCanvas]::new($Bitmap)

    $Entries = [System.Collections.Generic.List[Microcharts.ChartEntry]]::new()
    $OutputEntries = & $ChartEntries
    foreach ($Entry in $OutputEntries) {
        if ($Entry) {
            $Entries.Add($Entry)
        }
    }

    $BarChart = [Microcharts.LineChart]::new()
    # $BarChart.GraphPosition = [Microcharts.GraphPosition]::AutoFill
    $BarChart.Entries = $Entries
    $BarChart.DrawContent($Canvas, 500, 500)



    $Canvas.Save()
    $ChartImage = [System.IO.File]::Create($FilePath)
    $Image = [SkiaSharp.SKImage]::FromPixels($Bitmap.PeekPixels())
    $Data = $Image.Encode([SkiaSharp.SKEncodedImageFormat]::Png, 400)
    $Data.SaveTo($ChartImage)
    $ChartImage.Close()

}