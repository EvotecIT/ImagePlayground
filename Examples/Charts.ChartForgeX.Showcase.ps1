Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$samplesPath = Join-Path -Path $PSScriptRoot -ChildPath 'Samples'
if (-not (Test-Path -LiteralPath $samplesPath)) {
    New-Item -Path $samplesPath -ItemType Directory | Out-Null
}

$trendPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXTrend.png'
$trendSvg = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXTrend.svg'
$trendHtml = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXTrend.html'
$transparentPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXTransparent.png'
$overlayPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXOverlay.png'
$donutPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXDonut.png'
$progressPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXProgress.png'
$pictorialPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXPictorial.png'
$wordCloudPng = Join-Path -Path $samplesPath -ChildPath 'ChartsChartForgeXWordCloud.png'

$trendDefinitions = @(
    New-ImageChartLine -Name 'CPU' -Value 31,42,37,55,68,61,74,58,49,63 -Color DeepSkyBlue -Marker Circle -Smooth
    New-ImageChartLine -Name 'Memory' -Value 48,51,55,57,60,62,59,64,66,69 -Color MediumSeaGreen -Marker Square -Smooth
)

$trendOptions = New-ImageChartOptions -ShowLegend -LegendPosition Bottom -ShowDataLabels -TickCount 4
New-ImageChart -Definition $trendDefinitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendPng -Width 760 -Height 420
New-ImageChart -Definition $trendDefinitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendSvg -Width 760 -Height 420
New-ImageChart -Definition $trendDefinitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendHtml -Width 760 -Height 420

$transparentOptions = New-ImageChartOptions -Transparent -NoCard -NoPlotBackground -ShowLegend -LegendPosition Bottom -ShowDataLabels -TickCount 4
New-ImageChart -Definition $trendDefinitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $transparentOptions -FilePath $transparentPng -Width 760 -Height 420

$backgroundPath = Join-Path -Path $samplesPath -ChildPath 'Snow.jpeg'
$background = [ImagePlayground.Image]::Load($backgroundPath)
try {
    $background.AddImage($transparentPng, 80, 96, 1.0)
    $background.Save($overlayPng)
} finally {
    $background.Dispose()
}

$donutOptions = New-ImageChartOptions -ShowLegend -ShowPointLegend -LegendPosition Right -ShowDataLabels -DonutCenterValue '72%' -DonutCenterLabel 'Used'
New-ImageChart {
    New-ImageChartDonut -Name 'Used' -Value 72 -Color Crimson
    New-ImageChartDonut -Name 'Free' -Value 28 -Color MediumSeaGreen
} -Theme Dark -Options $donutOptions -FilePath $donutPng -Width 560 -Height 360

$progressOptions = New-ImageChartOptions -ProgressMaximum 100 -NoProgressHandles -ShowDataLabels
New-ImageChart {
    New-ImageChartProgress -Name 'Servers' -Value 96 -Color MediumSeaGreen
    New-ImageChartProgress -Name 'Workstations' -Value 89 -Color DeepSkyBlue
    New-ImageChartProgress -Name 'Laptops' -Value 74 -Color Orange
} -Theme Dark -Options $progressOptions -FilePath $progressPng -Width 560 -Height 320

$pictorialOptions = New-ImageChartOptions -PictorialSymbol Person -PictorialColumns 10 -ShowDataLabels
New-ImageChart {
    New-ImageChartPictorial -Name 'Running' -Value 9 -Color MediumSeaGreen
    New-ImageChartPictorial -Name 'Stopped' -Value 1 -Color Crimson
} -Theme Dark -Options $pictorialOptions -FilePath $pictorialPng -Width 560 -Height 260

New-ImageChart {
    New-ImageChartWordCloud -Name 'ChartForgeX' -Weight 24 -Color DeepSkyBlue
    New-ImageChartWordCloud -Name 'Transparent' -Weight 18 -Color MediumSeaGreen
    New-ImageChartWordCloud -Name 'SVG' -Weight 15 -Color Orange
    New-ImageChartWordCloud -Name 'PNG' -Weight 15 -Color Crimson
    New-ImageChartWordCloud -Name 'HTML' -Weight 15 -Color Purple
    New-ImageChartWordCloud -Name 'ImagePlayground' -Weight 20 -Color CornflowerBlue
} -Theme Light -Options (New-ImageChartOptions -WordCloudMaximumTerms 20) -FilePath $wordCloudPng -Width 620 -Height 360
