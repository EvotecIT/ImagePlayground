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

Describe 'ChartForgeX visual artifacts' {
    BeforeAll {
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'converts a native chart into an accessible reusable artifact' {
        $points = [ChartForgeX.Primitives.ChartPoint[]] @(
            [ChartForgeX.Primitives.ChartPoint]::new(1, 12)
            [ChartForgeX.Primitives.ChartPoint]::new(2, 18)
        )
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(320, 180).WithTitle('Requests')
        [void] $chart.AddLine('API', $points, [ChartForgeX.Primitives.ChartColor]::FromHex('#2563EB'))

        $artifact = $chart | ConvertTo-ImageVisualArtifact -Id request-chart -AccessibleDescription 'API requests over time'

        $artifact.GetType().FullName | Should -Be 'ChartForgeX.VisualArtifacts.VisualArtifact'
        $artifact.Id | Should -Be 'request-chart'
        $artifact.Accessibility.Description | Should -Be 'API requests over time'
        $artifact.SupportsExport([ChartForgeX.VisualArtifacts.VisualArtifactExportFormat]::Office) | Should -BeTrue
    }

    It 'exports aligned SVG and PNG watermarks with PNG DPI metadata' {
        $points = [ChartForgeX.Primitives.ChartPoint[]] @(
            [ChartForgeX.Primitives.ChartPoint]::new(1, 12)
            [ChartForgeX.Primitives.ChartPoint]::new(2, 18)
        )
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(320, 180)
        [void] $chart.AddBar('API', $points, [ChartForgeX.Primitives.ChartColor]::FromHex('#0F766E'))
        $artifact = $chart | ConvertTo-ImageVisualArtifact -Id watermarked-chart
        $watermark = New-ImageVisualWatermark -Text 'INTERNAL' -Anchor Center -Opacity 0.2 -RotationDegrees -24
        $svgPath = Join-Path $TestDir 'visual-artifact-watermark.svg'
        $pngPath = Join-Path $TestDir 'visual-artifact-watermark.png'

        $artifact | Export-ImageVisualArtifact -FilePath $svgPath -Watermark $watermark
        $artifact | Export-ImageVisualArtifact -FilePath $pngPath -Watermark $watermark -Dpi 144

        (Get-Content $svgPath -Raw) | Should -Match 'INTERNAL'
        $png = [System.IO.File]::ReadAllBytes($pngPath)
        [System.Text.Encoding]::ASCII.GetString($png) | Should -Match 'pHYs'
    }

    It 'uses the same artifact handoff for non-chart CFX canvases' {
        $canvas = [ChartForgeX.Composition.VisualCanvas]::Create(320, 180).WithTitle('Release overview')
        $artifact = $canvas | ConvertTo-ImageVisualArtifact -Id release-overview
        $svgPath = Join-Path $TestDir 'visual-canvas-office.svg'

        $artifact.Kind.ToString() | Should -Be 'VisualCanvas'
        $artifact.NaturalSize.Width | Should -Be 320
        $artifact | Export-ImageVisualArtifact -FilePath $svgPath
        (Get-Content $svgPath -Raw) | Should -Match '<svg'
    }

    It 'exposes a portable Office visual envelope without changing the artifact base type' {
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(160, 90)
        $artifact = $chart | ConvertTo-ImageVisualArtifact -Id portable-chart -Title 'Portable chart' -AccessibleDescription 'Portable chart description.'

        $artifact.GetType().FullName | Should -Be 'ChartForgeX.VisualArtifacts.VisualArtifact'
        $artifact.PSObject.TypeNames | Should -Contain 'ImagePlayground.VisualArtifact'
        $artifact.OfficeVisualSvg.Count | Should -BeGreaterThan 100
        $artifact.OfficeVisualId | Should -Be 'portable-chart'
        $artifact.OfficeVisualTitle | Should -Be 'Portable chart'
        $artifact.OfficeVisualAlternativeText | Should -Be 'Portable chart description.'

        $again = $artifact | ConvertTo-ImageVisualArtifact -Title 'Updated portable chart'
        $again.OfficeVisualTitle | Should -Be 'Updated portable chart'
        @($again.PSObject.TypeNames -eq 'ImagePlayground.VisualArtifact').Count | Should -Be 1
    }

    It 'rejects multiple pipeline artifacts for one output path' {
        $first = [ChartForgeX.Core.Chart]::Create().WithSize(120, 80) | ConvertTo-ImageVisualArtifact -Id first
        $second = [ChartForgeX.Core.Chart]::Create().WithSize(120, 80) | ConvertTo-ImageVisualArtifact -Id second
        $path = Join-Path $TestDir 'must-not-overwrite.svg'

        { @($first, $second) | Export-ImageVisualArtifact -FilePath $path -ErrorAction Stop } |
            Should -Throw '*exactly one artifact*'
    }

    It 'rejects unsupported image watermark formats instead of labelling them as PNG' {
        $path = Join-Path $TestDir 'unsupported.webp'
        [IO.File]::WriteAllBytes($path, [byte[]] @(82, 73, 70, 70, 0, 0, 0, 0, 87, 69, 66, 80))

        { New-ImageVisualWatermark -ImagePath $path -ErrorAction Stop } |
            Should -Throw '*support PNG, JPEG, BMP, GIF, TIFF, and PPM*'
    }
}
