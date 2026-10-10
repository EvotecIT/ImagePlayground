param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Samples'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Dark'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$samplesPath = $OutputDirectory
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
    New-ImageChartLine -Name 'CPU' -Value 31,42,37,55,68,61,74,58,49,63 -Marker Circle -Smooth
    New-ImageChartLine -Name 'Memory' -Value 48,51,55,57,60,62,59,64,66,69 -Marker Circle -Smooth
)

$trendOptions = New-ImageChartOptions -ShowLegend -LegendPosition Bottom -TickCount 4
New-ImageChart -Definition $trendDefinitions -Title 'Resource usage' -Subtitle 'Ten samples from one workstation' -Theme $Theme -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendPng -Width 760 -Height 420
New-ImageChart -Definition $trendDefinitions -Title 'Resource usage' -Subtitle 'Ten samples from one workstation' -Theme $Theme -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendSvg -Width 760 -Height 420
New-ImageChart -Definition $trendDefinitions -Title 'Resource usage' -Subtitle 'Ten samples from one workstation' -Theme $Theme -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $trendOptions -FilePath $trendHtml -Width 760 -Height 420

foreach ($extension in 'svg', 'png', 'html') {
    New-ImageChart -Definition $trendDefinitions -Title 'Resource usage' -Subtitle 'Ten workstation samples' -Theme $Theme -ShowGrid -XTitle Sample -YTitle 'Usage %' -Options $trendOptions -FilePath (Join-Path -Path $samplesPath -ChildPath "ChartsChartForgeXTrendCompact.$extension") -Width 368 -Height 320
}

$transparentOptions = New-ImageChartOptions -Transparent -NoCard -NoPlotBackground -ShowLegend -LegendPosition Bottom -TickCount 4
New-ImageChart -Definition $trendDefinitions -Title 'Resource usage' -Subtitle 'Ten samples from one workstation' -Theme $Theme -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $transparentOptions -FilePath $transparentPng -Width 760 -Height 420

# Use the standalone static background for this chart overlay.
$backgroundPath = Join-Path -Path $PSScriptRoot -ChildPath 'Samples/Snow.png'
$background = [ImagePlayground.Image]::Load($backgroundPath)
try {
    $background.AddImage($transparentPng, 80, 96, 1.0)
    $background.Save($overlayPng)
} finally {
    $background.Dispose()
}

$donutOptions = New-ImageChartOptions -ShowLegend -ShowPointLegend -LegendPosition Right -ShowDataLabels -DonutCenterValue '72%' -DonutCenterLabel 'Used'
New-ImageChart {
    New-ImageChartDonut -Name 'Used' -Value 72
    New-ImageChartDonut -Name 'Free' -Value 28
} -Theme $Theme -Options $donutOptions -FilePath $donutPng -Width 560 -Height 360

$progressOptions = New-ImageChartOptions -ProgressMaximum 100 -NoProgressHandles -ShowDataLabels
New-ImageChart {
    New-ImageChartProgress -Name 'Servers' -Value 96
    New-ImageChartProgress -Name 'Workstations' -Value 89
    New-ImageChartProgress -Name 'Laptops' -Value 74
} -Theme $Theme -Options $progressOptions -FilePath $progressPng -Width 560 -Height 320

$pictorialOptions = New-ImageChartOptions -PictorialSymbol Person -PictorialColumns 10 -ShowDataLabels
New-ImageChart {
    New-ImageChartPictorial -Name 'Running' -Value 9
    New-ImageChartPictorial -Name 'Stopped' -Value 1
} -Theme $Theme -Options $pictorialOptions -FilePath $pictorialPng -Width 560 -Height 260

New-ImageChart {
    New-ImageChartWordCloud -Name 'ChartForgeX' -Weight 24
    New-ImageChartWordCloud -Name 'Transparent' -Weight 18
    New-ImageChartWordCloud -Name 'SVG' -Weight 15
    New-ImageChartWordCloud -Name 'PNG' -Weight 15
    New-ImageChartWordCloud -Name 'HTML' -Weight 15
    New-ImageChartWordCloud -Name 'ImagePlayground' -Weight 20
} -Theme $Theme -Options (New-ImageChartOptions -WordCloudMaximumTerms 20) -FilePath $wordCloudPng -Width 620 -Height 360
