Describe 'New-ImageChart' {
    BeforeAll {
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
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
