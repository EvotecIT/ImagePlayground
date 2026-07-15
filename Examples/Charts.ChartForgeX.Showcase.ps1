$artifactModule = Get-ChildItem -Path "$PSScriptRoot\..\Artefacts\Unpacked\Modules\ImagePlayground" -Filter 'ImagePlayground.psd1' -ErrorAction SilentlyContinue | Select-Object -First 1
$modulePath = if ($artifactModule) { $artifactModule.FullName } else { 'ImagePlayground' }
Import-Module $modulePath -Force

$samplesPath = Join-Path -Path $PSScriptRoot -ChildPath 'Samples'
if (-not (Test-Path -LiteralPath $samplesPath)) {
    New-Item -Path $samplesPath -ItemType Directory | Out-Null
}

function New-ChartPointArray {
    param([double[]] $Values)

    $points = New-Object -TypeName 'ChartForgeX.Primitives.ChartPoint[]' -ArgumentList $Values.Length
    for ($index = 0; $index -lt $Values.Length; $index++) {
        $points[$index] = New-Object -TypeName ChartForgeX.Primitives.ChartPoint -ArgumentList ([double] ($index + 1)), $Values[$index]
    }
    Write-Output -NoEnumerate $points
}

$cpu = New-ChartPointArray 31, 42, 37, 55, 68, 61, 74, 58, 49, 63
$memory = New-ChartPointArray 48, 51, 55, 57, 60, 62, 59, 64, 66, 69
$chart = New-Object -TypeName ChartForgeX.Core.Chart
$themeType = $chart.GetType().Assembly.GetType('ChartForgeX.Themes.ChartTheme', $true)
$theme = $themeType.GetMethod('ReportDark').Invoke($null, $null)
$chart.WithTitle('Workstation health').WithSubtitle('Native ChartForgeX API rendered through ImagePlayground').WithSize(920, 520).WithTheme($theme).WithXAxis('Sample').WithYAxis('Usage %').WithXLabels('1', '2', '3', '4', '5', '6', '7', '8', '9', '10').WithGrid().WithLegend() | Out-Null
$chart.AddSmoothLine('CPU', $cpu) | Out-Null
$chart.AddSmoothLine('Memory', $memory) | Out-Null
$chart.AddHorizontalLine(80, 'review threshold') | Out-Null

$chart | New-ImageChart -FilePath (Join-Path $samplesPath 'ChartsChartForgeXTrend.png')
$chart | New-ImageChart -FilePath (Join-Path $samplesPath 'ChartsChartForgeXTrend.svg')
$chart | New-ImageChart -FilePath (Join-Path $samplesPath 'ChartsChartForgeXTrend.html')
