BeforeDiscovery {
    $artifactModule = Get-ChildItem -Path "$PSScriptRoot/../Artefacts/Unpacked/Modules/ImagePlayground" -Filter 'ImagePlayground.psd1' -ErrorAction SilentlyContinue | Select-Object -First 1
    $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
        $env:IMAGEPLAYGROUND_TEST_MODULE_PATH
    } elseif ($artifactModule) {
        $artifactModule.FullName
    } else {
        "$PSScriptRoot/../ImagePlayground.psd1"
    }
    Import-Module -Name $modulePath -Force
}

Describe 'New-ImageChart' {
    BeforeAll {
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'exports the complete ImagePlayground chart command surface' {
        $expectedCommands = @(
            'New-ImageChart'
            'New-ImageChartAnnotation'
            'New-ImageChartArea'
            'New-ImageChartBar'
            'New-ImageChartBarOptions'
            'New-ImageChartBoxPlot'
            'New-ImageChartBubble'
            'New-ImageChartBullet'
            'New-ImageChartCircle'
            'New-ImageChartDonut'
            'New-ImageChartFunnel'
            'New-ImageChartGauge'
            'New-ImageChartHeatmap'
            'New-ImageChartHistogram'
            'New-ImageChartHorizontalBar'
            'New-ImageChartLine'
            'New-ImageChartLollipop'
            'New-ImageChartOptions'
            'New-ImageChartPictorial'
            'New-ImageChartPie'
            'New-ImageChartPolar'
            'New-ImageChartProgress'
            'New-ImageChartRadar'
            'New-ImageChartRadial'
            'New-ImageChartRangeBand'
            'New-ImageChartRangeBar'
            'New-ImageChartScatter'
            'New-ImageChartSlope'
            'New-ImageChartStackedArea'
            'New-ImageChartStepArea'
            'New-ImageChartStepLine'
            'New-ImageChartTreemap'
            'New-ImageChartWaterfall'
            'New-ImageChartWordCloud'
        )

        $actualCommands = Get-Command -Module ImagePlayground -Name 'New-ImageChart*' |
            Select-Object -ExpandProperty Name |
            Sort-Object

        Compare-Object -ReferenceObject ($expectedCommands | Sort-Object) -DifferenceObject $actualCommands |
            Should -BeNullOrEmpty
    }

    It 'preserves repeated treemap labels and fractional values with distinct item identities' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-treemap-identities.svg'
        $definitions = @(
            New-ImageChartTreemap -Name 'Shared service' -Value 2.5
            New-ImageChartTreemap -Name 'Shared service' -Value 7.5
        )
        $chart = New-ImageChart -Definition $definitions -FilePath $file -Width 480 -Height 320 -PassThru

        $items = @($chart.Series[0].HierarchyItems)
        $items.Count | Should -Be 2
        @($items.Id | Select-Object -Unique).Count | Should -Be 2
        $items[0].Label | Should -BeExactly 'Shared service'
        $items[1].Label | Should -BeExactly 'Shared service'
        $items[0].Value | Should -Be 2.5
        $items[1].Value | Should -Be 7.5
        [System.IO.File]::ReadAllText($file) | Should -Match 'Shared service'
    }

    It 'applies the radial center label option to the progress ring model' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-radial-center.svg'
        $options = New-ImageChartOptions
        $options.ShowRadialBarCenterLabel = $false
        $definition = New-ImageChartRadial -Name 'CPU' -Value 75
        $chart = New-ImageChart -Definition $definition -Options $options -FilePath $file -Width 480 -Height 320 -PassThru

        $chart.Options.ShowProgressRingCenterLabel | Should -BeFalse
        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'creates a bar chart' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart with axis titles' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_titles.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        } -FilePath $file -Width 200 -Height 150 -XTitle 'X' -YTitle 'Y'

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart with grid lines' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_grid.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        } -FilePath $file -Width 200 -Height 150 -ShowGrid

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart with background color' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_background.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        } -FilePath $file -Width 200 -Height 150 -Background ([OfficeIMO.Drawing.OfficeColor]::Aqua)

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a polar chart' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_polar.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartPolar -Name 'S1' -Angle @(0, 1, 2) -Value @(1, 2, 1)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a radar chart through the public definition command' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_radar.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartRadar -Name 'Current' -Category 1, 2, 3, 4 -Value 82, 68, 91, 74 -Color '#2563EB'
            New-ImageChartRadar -Name 'Target' -Category 1, 2, 3, 4 -Value 90, 85, 88, 92 -Color '#14B8A6'
        } -FilePath $file -Width 500 -Height 360 -Theme Aurora

        Test-Path -Path $file | Should -BeTrue
        (Get-Content -Path $file -Raw) | Should -Match 'Current'
        [enum]::GetNames([ImagePlayground.ChartTheme]) | Should -Contain 'Colorblind'
        [enum]::GetNames([ImagePlayground.ChartTheme]) | Should -Contain 'DashboardLight'
    }

    It 'preserves true polar angle and radius data in a dedicated definition' {
        $definition = New-ImageChartPolar -Name 'Irregular sweep' -Angle @(0, 0.7, 2.4, 5.7) -Value @(1, 4, 2, 3) -Color '#38BDF8'

        $definition.GetType().FullName | Should -Be 'ImagePlayground.ChartPolar'
        $definition.Angle.Count | Should -Be 4
        $definition.Angle[0] | Should -Be 0
        $definition.Angle[1] | Should -Be 0.7
        $definition.Angle[2] | Should -Be 2.4
        $definition.Angle[3] | Should -Be 5.7
        $definition.Radius.Count | Should -Be 4
        $definition.Radius[0] | Should -Be 1
        $definition.Radius[1] | Should -Be 4
        $definition.Radius[2] | Should -Be 2
        $definition.Radius[3] | Should -Be 3
    }

    It 'preserves explicit line marker semantics' {
        $withoutMarkers = New-ImageChartLine -Name 'Plain' -Value 1, 3, 2
        $withMarkers = New-ImageChartLine -Name 'Marked' -Value 2, 4, 3 -Marker Circle

        $sizedOnly = New-ImageChartLine -Name 'Sized' -Value 2, 4, 3 -MarkerSize 9
        $diamond = New-ImageChartLine -Name 'Diamond' -Value 2, 4, 3 -Marker Diamond -MarkerSize 10

        $withoutMarkers.MarkerSize | Should -Be 0
        $withoutMarkers.MarkerShape | Should -Be 'None'
        $withMarkers.MarkerSize | Should -Be 6
        $sizedOnly.MarkerShape | Should -Be 'Circle'
        $sizedOnly.MarkerSize | Should -Be 9
        $diamond.MarkerShape | Should -Be 'Diamond'
        [enum]::GetNames([ImagePlayground.ChartMarkerShape]) | Should -Be @('None', 'Circle', 'Square', 'Diamond', 'Triangle', 'Plus', 'Cross', 'Star')
    }

    It 'configures markers per line series so marked and plain lines can share a chart' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_mixed_line_markers.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $chart = New-ImageChart -ChartsDefinition {
            New-ImageChartLine -Name 'Plain' -Value 1, 3, 2
            New-ImageChartLine -Name 'Marked' -Value 2, 4, 3 -Marker Square -MarkerSize 10
        } -FilePath $file -PassThru

        Test-Path -Path $file | Should -BeTrue
        $chart.Series[0].Markers.Enabled | Should -BeFalse
        $chart.Series[1].Markers.Enabled | Should -BeTrue
        $chart.Series[1].Markers.Shape.ToString() | Should -Be 'Square'
        $chart.Series[1].Markers.Radius | Should -Be 5
    }

    It 'preserves the requested histogram bin boundaries and observation counts' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_histogram_bin_size.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $chart = New-ImageChart -ChartsDefinition {
            New-ImageChartHistogram -Name 'Requested width' -Values 0, 1, 3, 5, 6, 9, 10 -BinSize 3
        } -FilePath $file -Width 500 -Height 320 -PassThru

        $bins = @($chart.Series[0].HistogramBins)
        $bins.Count | Should -Be 4
        for ($index = 0; $index -lt $bins.Count; $index++) {
            $bins[$index].LowerBound | Should -Be (3 * $index)
            $bins[$index].UpperBound | Should -Be (3 * ($index + 1))
            $bins[$index].Width | Should -Be 3
        }
        $bins[0].Count | Should -Be 2
        $bins[1].Count | Should -Be 2
        $bins[2].Count | Should -Be 1
        $bins[3].Count | Should -Be 2
        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'rejects point callouts for exclusive polar charts before rendering' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_polar_annotation.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        {
            New-ImageChart -ChartsDefinition {
                New-ImageChartPolar -Name 'Sweep' -Angle @(0, 0.7, 2.4, 5.7) -Value @(1, 4, 2, 3)
            } -AnnotationsDefinition {
                New-ImageChartAnnotation -X 0.7 -Y 4 -Text 'Peak' -Arrow
            } -FilePath $file -ErrorAction Stop
        } | Should -Throw '*exclusive chart kinds*'

        Test-Path -Path $file | Should -BeFalse
    }

    It 'creates an area chart' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_area.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartArea -Name 'S1' -Value @(1,2,3)
            New-ImageChartArea -Name 'S2' -Value @(2,4,6)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart from definitions' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_defs.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $defs = @(
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        )

        New-ImageChart -Definition $defs -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart from pipeline input' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_pipe.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $defs = @(
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        )

        $defs | New-ImageChart -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'renders a ChartForgeX chart object' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_chartforgex_object.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $points = [ChartForgeX.Primitives.ChartPoint[]] @(
            [ChartForgeX.Primitives.ChartPoint]::new(1, 10)
            [ChartForgeX.Primitives.ChartPoint]::new(2, 14)
            [ChartForgeX.Primitives.ChartPoint]::new(3, 9)
        )
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(200, 150)
        [void] $chart.AddLine('Latency', $points, [ChartForgeX.Primitives.ChartColor]::FromHex('#2563EB'))

        New-ImageChart -Chart $chart -FilePath $file

        Test-Path -Path $file | Should -BeTrue
    }

    It 'renders a ChartForgeX chart script' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_chartforgex_script.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartScript {
            param($Chart)

            $points = [ChartForgeX.Primitives.ChartPoint[]] @(
                [ChartForgeX.Primitives.ChartPoint]::new(1, 4)
                [ChartForgeX.Primitives.ChartPoint]::new(2, 8)
                [ChartForgeX.Primitives.ChartPoint]::new(3, 6)
            )
            [void] $Chart.AddBar('Requests', $points, [ChartForgeX.Primitives.ChartColor]::FromHex('#14B8A6'))
        } -FilePath $file -Width 200 -Height 150 -XTitle 'Minute' -YTitle 'Count'

        Test-Path -Path $file | Should -BeTrue
    }

    It 'renders a native chart from the pipeline without changing its model or presentation' {
        $authoredFile = Join-Path -Path $TestDir -ChildPath 'chart-native-authored.svg'
        $pipelineFile = Join-Path -Path $TestDir -ChildPath 'chart-native-pipeline.svg'
        $definition = New-ImageChartLine -Name Builds -Value 12, 18, 15, 24, 31
        $authored = New-ImageChart -Definition $definition -Theme Dark -Width 368 -Height 320 -Title 'Weekly builds' -Subtitle 'Completed pipeline runs' -FilePath $authoredFile -PassThru
        $expectedSvg = [System.IO.File]::ReadAllText($authoredFile)

        $rendered = $authored | New-ImageChart -FilePath $pipelineFile -PassThru

        [object]::ReferenceEquals($authored, $rendered) | Should -BeTrue
        [System.IO.File]::ReadAllText($pipelineFile) | Should -BeExactly $expectedSvg
    }

    It 'sets headings consistently and preserves unbound native chart headings' {
        $definition = New-ImageChartLine -Name Builds -Value 12, 18, 15, 24, 31
        $authored = New-ImageChart -Definition $definition -Title 'Weekly builds' -Subtitle 'Completed pipeline runs' -FilePath (Join-Path -Path $TestDir -ChildPath 'chart-headings.svg') -PassThru
        $authored.Title | Should -BeExactly 'Weekly builds'
        $authored.Subtitle | Should -BeExactly 'Completed pipeline runs'

        $preserved = New-ImageChart -Chart $authored -FilePath (Join-Path -Path $TestDir -ChildPath 'chart-headings-preserved.svg') -PassThru
        $preserved.Title | Should -BeExactly 'Weekly builds'
        $preserved.Subtitle | Should -BeExactly 'Completed pipeline runs'
        $overridden = New-ImageChart -Chart $authored -Title 'Build history' -Subtitle '' -FilePath (Join-Path -Path $TestDir -ChildPath 'chart-headings-overridden.svg') -PassThru
        $overridden.Title | Should -BeExactly 'Build history'
        $overridden.Subtitle | Should -BeNullOrEmpty
        [System.IO.File]::ReadAllText((Join-Path -Path $TestDir -ChildPath 'chart-headings-overridden.svg')) | Should -Match 'Build history'
    }

    It 'accepts ChartForgeX-style color names and hex values' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_chartforgex_colors.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $options = New-ImageChartOptions -Palette '#2563EB', 'Orange' -Transparent -NoCard -NoPlotBackground
        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1, 2, 3) -Color '#2563EB'
            New-ImageChartBar -Name 'Feb' -Value @(3, 2, 1) -Color Orange
        } -FilePath $file -Width 200 -Height 150 -Options $options

        Test-Path -Path $file | Should -BeTrue
    }

    It 'renders identical output for multiple definitions supplied as an array or pipeline' {
        $arrayFile = Join-Path -Path $TestDir -ChildPath 'chart_array_compare.png'
        $pipeFile = Join-Path -Path $TestDir -ChildPath 'chart_pipe_compare.png'
        if (Test-Path -Path $arrayFile) {
            Remove-Item -Path $arrayFile
        }
        if (Test-Path -Path $pipeFile) {
            Remove-Item -Path $pipeFile
        }

        $defs = @(
            New-ImageChartBar -Name 'Jan' -Value @(1, 2)
            New-ImageChartBar -Name 'Feb' -Value @(3, 4)
        )

        New-ImageChart -Definition $defs -FilePath $arrayFile -Width 200 -Height 150
        $defs | New-ImageChart -FilePath $pipeFile -Width 200 -Height 150

        $first = [ImagePlayground.Image]::Load($arrayFile)
        $second = [ImagePlayground.Image]::Load($pipeFile)
        $comparison = $first.Compare($second)

        $comparison.ChangedPixels | Should -Be 0

        $first.Dispose()
        $second.Dispose()
    }

    It 'combines bar and line definitions in definition order through <InputKind> input' -TestCases @(
        @{ InputKind = 'Array' }
        @{ InputKind = 'Pipeline' }
    ) {
        param($InputKind)

        $file = Join-Path -Path $TestDir -ChildPath ('chart-mixed-{0}.svg' -f $InputKind)
        Remove-Item -Path $file -ErrorAction SilentlyContinue
        $definitions = @(
            New-ImageChartBar -Name Requests -Value 10, 20
            New-ImageChartLine -Name Latency -Value 4, 8
        )

        $chart = if ($InputKind -eq 'Array') {
            New-ImageChart -Definition $definitions -FilePath $file -PassThru
        } else {
            $definitions | New-ImageChart -FilePath $file -PassThru
        }
        Test-Path -Path $file | Should -BeTrue
        @($chart.Series | ForEach-Object { $_.Kind.ToString() }) | Should -Be @('Bar', 'Line')
        @($chart.Series | ForEach-Object { $_.Name }) | Should -Be @('Requests', 'Latency')
    }

    It 'rejects chart kinds that cannot share one chart before creating output' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-mixed-rejected.svg'
        Remove-Item -Path $file -ErrorAction SilentlyContinue

        {
            New-ImageChart -ChartsDefinition {
                New-ImageChartPie -Name Web -Value 10
                New-ImageChartBar -Name Requests -Value 10, 20
            } -FilePath $file -ErrorAction Stop
        } | Should -Throw '*cannot share one chart*'
        Test-Path -Path $file | Should -BeFalse
    }

    It 'plots series flagged with -SecondaryAxis against the secondary axis' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-secondary-axis.svg'
        $chart = New-ImageChart -XLabel Mon, Tue, Wed -SecondaryYTitle 'Error rate (%)' -FilePath $file -PassThru {
            New-ImageChartBar -Name Requests -Value 410, 380, 455
            New-ImageChartLine -Name 'Error rate' -Value 1.2, 0.8, 2.4 -SecondaryAxis -Marker Circle
            New-ImageChartArea -Name Baseline -Value 1, 1, 1 -SecondaryAxis
        }

        $chart.Series[0].YAxis.ToString() | Should -Be 'Primary'
        $chart.Series[1].YAxis.ToString() | Should -Be 'Secondary'
        $chart.Series[2].YAxis.ToString() | Should -Be 'Secondary'
        Get-Content -Path $file -Raw | Should -Match 'Error rate \(%\)'
    }

    It 'applies titles, category labels, and stacked bars through parameters' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-stacked-labels.svg'
        $chart = New-ImageChart -Title 'Builds by result' -Subtitle 'Week 41' -XLabel Mon, Tue, Wed -FilePath $file -PassThru -Options (New-ImageChartOptions -BarLayout Stacked) {
            New-ImageChartBar -Name Passed -Value 41, 38, 45
            New-ImageChartBar -Name Failed -Value 3, 6, 2
        }

        $chart.Title | Should -Be 'Builds by result'
        $chart.Subtitle | Should -Be 'Week 41'
        @($chart.Options.XAxisLabels | ForEach-Object { $_.Text }) | Should -Be @('Mon', 'Tue', 'Wed')
        $chart.Options.BarMode.ToString() | Should -Be 'Stacked'
        $svg = Get-Content -Path $file -Raw
        $svg | Should -Match 'Builds by result'
        $svg | Should -Match 'Tue'
    }

    It 'labels index-based categories with whole numbers when no labels are supplied' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-default-labels.svg'
        $chart = New-ImageChart -FilePath $file -PassThru { New-ImageChartBar -Name A -Value 3, 5, 2 }

        @($chart.Options.XAxisLabels | ForEach-Object { $_.Text }) | Should -Be @('1', '2', '3')
    }

    It 'writes <Extension> output through the ImagePlayground encoder' -TestCases @(
        @{ Extension = 'webp' }
        @{ Extension = 'jpg' }
        @{ Extension = 'gif' }
    ) {
        param($Extension)
        $file = Join-Path -Path $TestDir -ChildPath "chart-encoded.$Extension"
        Remove-Item -Path $file -ErrorAction SilentlyContinue

        New-ImageChart -FilePath $file -Width 1200 -Height 300 { New-ImageChartBar -Name A -Value 3, 5, 2 }

        Test-Path -Path $file | Should -BeTrue
        $image = Get-Image -FilePath $file
        try {
            $image.Width | Should -Be 1200
            $image.Height | Should -Be 300
        } finally {
            $image.Dispose()
        }
    }

    It 'creates parent directory when saving a chart' {
        $folder = Join-Path -Path $TestDir -ChildPath 'NestedChart'
        $file = Join-Path -Path $folder -ChildPath 'chart.png'
        if (Test-Path -Path $folder) {
            Remove-Item -Path $folder -Recurse -Force
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1, 2)
            New-ImageChartBar -Name 'Feb' -Value @(3, 4)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }
}
