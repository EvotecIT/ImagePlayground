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
        } -FilePath $file -Width 200 -Height 150 -Background ([SixLabors.ImageSharp.Color]::Aqua)

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

        $withoutMarkers.MarkerSize | Should -Be 0
        $withMarkers.MarkerSize | Should -Be 6
        [enum]::GetNames([ImagePlayground.ChartMarkerShape]) | Should -Be @('None', 'Circle')
    }

    It 'rejects mixed line marker policies before rendering' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_mixed_line_markers.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        {
            New-ImageChart -ChartsDefinition {
                New-ImageChartLine -Name 'Plain' -Value 1, 3, 2
                New-ImageChartLine -Name 'Marked' -Value 2, 4, 3 -Marker Circle
            } -FilePath $file -ErrorAction Stop
        } | Should -Throw '*shared marker size*'

        Test-Path -Path $file | Should -BeFalse
    }

    It 'preserves the requested histogram bin size in rendered labels' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_histogram_bin_size.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartHistogram -Name 'Requested width' -Values 0, 1, 3, 5, 6, 9, 10 -BinSize 3
        } -FilePath $file -Width 500 -Height 320

        $svg = Get-Content -Path $file -Raw
        $svg | Should -Match '>0-3</text>'
        $svg | Should -Match '>3-6</text>'
        $svg | Should -Match '>6-9</text>'
        $svg | Should -Match '>9-10</text>'
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

    It 'renders identical output for array and pipeline input' {
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

        $comparison.PixelErrorCount | Should -Be 0

        $first.Dispose()
        $second.Dispose()
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
