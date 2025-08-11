Describe 'New-ImageChart' {
    BeforeAll {
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'creates a bar chart' -Skip:(-not $IsWindows) {
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

    It 'creates a bar chart with axis titles' -Skip:(-not $IsWindows) {
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

    It 'creates a bar chart with grid lines' -Skip:(-not $IsWindows) {
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

    It 'creates a bar chart with background color' -Skip:(-not $IsWindows) {
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

    It 'creates a polar chart' -Skip:(-not $IsWindows) {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_polar.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartPolar -Name 'S1' -Angle @(0,1) -Value @(1,2)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }

    It 'creates a bar chart from definitions' -Skip:(-not $IsWindows) {
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

    It 'creates a bar chart from pipeline input' -Skip:(-not $IsWindows) {
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
}
