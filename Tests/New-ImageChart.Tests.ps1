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
        New-Item -Path $TestDir -ItemType Directory -Force | Out-Null
    }

    It 'renders a script-configured native ChartForgeX chart' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-script.png'

        New-ImageChart -ChartScript {
            param($chart)
            $chart.WithTitle('Projects').WithSubtitle('Configured by script').WithSize(360, 240)
        } -FilePath $file

        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'renders exactly one native chart from the pipeline' {
        $file = Join-Path -Path $TestDir -ChildPath 'chart-pipeline.svg'
        $chart = New-Object -TypeName ChartForgeX.Core.Chart
        $points = New-Object -TypeName 'ChartForgeX.Primitives.ChartPoint[]' -ArgumentList 3
        $points[0] = New-Object -TypeName ChartForgeX.Primitives.ChartPoint -ArgumentList 1.0, 10.0
        $points[1] = New-Object -TypeName ChartForgeX.Primitives.ChartPoint -ArgumentList 2.0, 14.0
        $points[2] = New-Object -TypeName ChartForgeX.Primitives.ChartPoint -ArgumentList 3.0, 9.0
        $chart.WithSize(360, 240).AddSmoothLine('Latency', $points) | Out-Null

        $chart | New-ImageChart -FilePath $file

        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'rejects several charts for one output path' {
        $file = Join-Path -Path $TestDir -ChildPath 'invalid.png'
        $charts = @(
            New-Object -TypeName ChartForgeX.Core.Chart
            New-Object -TypeName ChartForgeX.Core.Chart
        )

        { $charts | New-ImageChart -FilePath $file -ErrorAction Stop } | Should -Throw
    }
}
