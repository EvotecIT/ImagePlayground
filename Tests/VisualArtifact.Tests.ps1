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
        $beforePath = Join-Path $TestDir 'visual-artifact-before.svg'
        $afterPath = Join-Path $TestDir 'visual-artifact-after.svg'
        $artifact | Export-ImageVisualArtifact -FilePath $beforePath
        $sourceSvg = [System.IO.File]::ReadAllText($beforePath)
        $sourceRender = $artifact.RenderSource

        $artifact | Export-ImageVisualArtifact -FilePath $svgPath -Watermark $watermark
        $artifact | Export-ImageVisualArtifact -FilePath $pngPath -Watermark $watermark -Dpi 144

        (Get-Content $svgPath -Raw) | Should -Match 'INTERNAL'
        $png = [System.IO.File]::ReadAllBytes($pngPath)
        [System.Text.Encoding]::ASCII.GetString($png) | Should -Match 'pHYs'
        $artifact | Export-ImageVisualArtifact -FilePath $afterPath
        [System.IO.File]::ReadAllText($afterPath) | Should -BeExactly $sourceSvg
        [object]::ReferenceEquals($artifact.RenderSource, $sourceRender) | Should -BeTrue

        $imageMark = New-ImageVisualWatermark -ImagePath $pngPath -Anchor BottomRight -Opacity 0.4 -Scale 0.1 -RotationDegrees 12 -Repeat
        $imageSvgPath = Join-Path $TestDir 'visual-artifact-image-watermark.svg'
        $imagePngPath = Join-Path $TestDir 'visual-artifact-image-watermark.png'
        $artifact | Export-ImageVisualArtifact -FilePath $imageSvgPath -Watermark $imageMark
        $artifact | Export-ImageVisualArtifact -FilePath $imagePngPath -Watermark $imageMark
        (Get-Content -LiteralPath $imageSvgPath -Raw) | Should -Match 'data:image/png;base64'
        [IO.File]::ReadAllBytes($imagePngPath).Length | Should -BeGreaterThan 64
        [object]::ReferenceEquals($artifact.RenderSource, $sourceRender) | Should -BeTrue
    }

    It 'preserves declaration order and keeps repeated exports independent' {
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(320, 180).WithTitle('Reuse')
        [void] $chart.AddBar('API', [ChartForgeX.Primitives.ChartPoint[]] @([ChartForgeX.Primitives.ChartPoint]::new(1, 12)))
        $artifact = $chart | ConvertTo-ImageVisualArtifact -Id reusable-export
        $first = New-ImageVisualWatermark -Text FIRST -Anchor TopLeft
        $second = New-ImageVisualWatermark -Text SECOND -Anchor BottomRight
        $orderedPath = Join-Path $TestDir 'ordered-watermarks.svg'
        $secondPath = Join-Path $TestDir 'independent-watermark.svg'

        $returned = $artifact | Export-ImageVisualArtifact -FilePath $orderedPath -Watermark $first, $second -PassThru
        $artifact | Export-ImageVisualArtifact -FilePath $secondPath -Watermark $second

        [object]::ReferenceEquals($returned, $artifact) | Should -BeTrue
        $ordered = Get-Content -LiteralPath $orderedPath -Raw
        $firstPosition = $ordered.IndexOf('FIRST', [StringComparison]::Ordinal)
        $secondPosition = $ordered.IndexOf('SECOND', [StringComparison]::Ordinal)
        $firstPosition | Should -BeGreaterThan -1
        $secondPosition | Should -BeGreaterThan -1
        $firstPosition | Should -BeLessThan $secondPosition
        (Get-Content -LiteralPath $secondPath -Raw) | Should -Not -Match 'FIRST'
    }

    It 'accepts a prepared chart through the common static artifact contract' {
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(320, 180).WithTitle('Prepared request count')
        [void] $chart.AddBar('API', [ChartForgeX.Primitives.ChartPoint[]] @([ChartForgeX.Primitives.ChartPoint]::new(1, 12)))
        $layout = [ChartForgeX.Rendering.VisualLayoutOptions]::new([ChartForgeX.Rendering.VisualSize]::new(320, 180))
        $prepared = $chart.Prepare([ChartForgeX.Rendering.VisualRenderContext]::new($layout))

        $artifact = $prepared | ConvertTo-ImageVisualArtifact -Id prepared-chart

        $artifact.Model | Should -BeOfType 'ChartForgeX.Rendering.PreparedVisual'
        $artifact.NaturalSize.Width | Should -Be $prepared.Size.Width
        [Text.Encoding]::UTF8.GetString($artifact.OfficeVisualSvg) | Should -Match 'Prepared request count'
        $artifact.OfficeVisualKind | Should -Be 'Unknown'
    }

    It 'uses the shared grid artifact producer with native dimensions and accessibility' {
        $grid = New-ImageVisualGrid -Title 'Service status' -Content (New-ImageMetricCard -Label Ready -Value Yes)

        $artifact = $grid | ConvertTo-ImageVisualArtifact -Id static-grid

        $artifact.Model | Should -BeOfType 'ChartForgeX.VisualBlocks.VisualGrid'
        $artifact.OfficeVisualKind | Should -Be 'ChartGrid'
        $artifact.NaturalSize.Width | Should -BeGreaterThan 0
        $artifact.NaturalSize.Height | Should -BeGreaterThan 0
        $artifact.Accessibility.Name | Should -Be 'Service status'
        [Text.Encoding]::UTF8.GetString($artifact.OfficeVisualSvg) | Should -Match 'Ready'
        $artifact.SupportsExport([ChartForgeX.VisualArtifacts.VisualArtifactExportFormat]::Office) | Should -BeTrue
    }

    It 'normalizes null-only bindings and rejects mixed null watermark entries across visual cmdlets' {
        $chart = [ChartForgeX.Core.Chart]::Create().WithSize(160, 90)
        [void] $chart.AddLine('API', [ChartForgeX.Primitives.ChartPoint[]] @([ChartForgeX.Primitives.ChartPoint]::new(1, 1)))
        $artifact = $chart | ConvertTo-ImageVisualArtifact -Id null-watermark
        $artifactPath = Join-Path $TestDir 'null-watermark-artifact.svg'
        $chartPath = Join-Path $TestDir 'null-watermark-chart.svg'
        $topologyPath = Join-Path $TestDir 'null-watermark-topology.svg'

        $artifact | Export-ImageVisualArtifact -FilePath $artifactPath -Watermark (,$null) -ErrorAction Stop
        New-ImageChart -ChartsDefinition { New-ImageChartLine -Name API -Value 1, 2 } -Watermark (,$null) -FilePath $chartPath -ErrorAction Stop
        New-ImageTopology -Node (New-ImageTopologyNode -Id api -Label API) -Watermark (,$null) -FilePath $topologyPath -ErrorAction Stop

        Test-Path -LiteralPath $artifactPath | Should -BeTrue
        Test-Path -LiteralPath $chartPath | Should -BeTrue
        Test-Path -LiteralPath $topologyPath | Should -BeTrue

        $mixed = [ChartForgeX.VisualArtifacts.VisualWatermark[]] @((New-ImageVisualWatermark -Text INTERNAL), $null)
        { $artifact | Export-ImageVisualArtifact -FilePath $artifactPath -Watermark $mixed -ErrorAction Stop } | Should -Throw '*null entries*'
        { New-ImageChart -ChartsDefinition { New-ImageChartLine -Name API -Value 1 } -Watermark $mixed -FilePath $chartPath -ErrorAction Stop } | Should -Throw '*null entries*'
        { New-ImageTopology -Node (New-ImageTopologyNode -Id api -Label API) -Watermark $mixed -FilePath $topologyPath -ErrorAction Stop } | Should -Throw '*null entries*'
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
        $artifact.OfficeVisualInterchangeJson.Count | Should -BeGreaterThan 100
        $artifact.OfficeVisualInterchangeSchema | Should -Be 'chartforgex.visual-artifact'
        $artifact.OfficeVisualInterchangeVersion | Should -Be 1
        $artifact.OfficeVisualKind | Should -Be 'Chart'
        $artifact.OfficeVisualId | Should -Be 'portable-chart'
        $artifact.OfficeVisualTitle | Should -Be 'Portable chart'
        $artifact.OfficeVisualAlternativeText | Should -Be 'Portable chart description.'

        $again = $artifact | ConvertTo-ImageVisualArtifact -Title 'Updated portable chart'
        $again.OfficeVisualTitle | Should -Be 'Updated portable chart'
        @($again.PSObject.TypeNames -eq 'ImagePlayground.VisualArtifact').Count | Should -Be 1
    }

    It 'preserves native topology semantics in the portable Office interchange payload' {
        $topologyPath = Join-Path $TestDir 'portable-topology.svg'
        $topology = New-ImageTopology -TopologyDefinition {
            New-ImageTopologyNode -Id api -Label API
            New-ImageTopologyNode -Id database -Label Database
            New-ImageTopologyEdge -Id api-db -SourceNodeId api -TargetNodeId database -Label queries
        } -FilePath $topologyPath -NoTitle -PassThru

        $artifact = $topology | ConvertTo-ImageVisualArtifact -Id portable-topology -Title 'Portable topology'
        $envelope = [Text.Encoding]::UTF8.GetString($artifact.OfficeVisualInterchangeJson) | ConvertFrom-Json

        $envelope.kind | Should -Be 'Topology'
        $envelope.family | Should -Be 'Topology'
        $envelope.topology.layoutMode | Should -Be 'Layered'
        $envelope.topology.layoutDirection | Should -Be 'TopToBottom'
        $envelope.Nodes.Count | Should -Be 2
        $envelope.Nodes[0].role | Should -Be 'TopologyNode'
        $envelope.Nodes[0].topology.kind | Should -Not -BeNullOrEmpty
        $envelope.Edges.Count | Should -Be 1
        $envelope.Edges[0].role | Should -Be 'TopologyEdge'
        $envelope.Edges[0].topology.kind | Should -Not -BeNullOrEmpty
        $envelope.Edges[0].sourceId | Should -Be 'api'
        $envelope.Edges[0].targetId | Should -Be 'database'
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
