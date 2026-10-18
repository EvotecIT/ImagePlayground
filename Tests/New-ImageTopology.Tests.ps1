Describe 'New-ImageTopology' {
    BeforeAll {
        if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            Import-Module -Name $env:IMAGEPLAYGROUND_TEST_MODULE_PATH -Force
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        }
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'creates a transparent topology PNG' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageTopology -TopologyDefinition {
            New-ImageTopologyGroup -Id 'lab' -Label 'Lab' -Status Healthy -Symbol region
            New-ImageTopologyNode -Id 'gateway' -Label 'Gateway' -Kind Network -Status Healthy -GroupId 'lab' -Symbol GW
            New-ImageTopologyNode -Id 'api' -Label 'API' -Kind Service -Status Healthy -GroupId 'lab' -Symbol API
            New-ImageTopologyEdge -SourceNodeId 'gateway' -TargetNodeId 'api' -Label 'HTTPS' -Kind Connectivity -Status Healthy -Direction Forward
        } -Title 'Lab topology' -Layout Layered -Direction LeftToRight -Theme Dark -Transparent -FilePath $file -Width 480 -Height 260

        Test-Path -Path $file | Should -BeTrue
        $bytes = [System.IO.File]::ReadAllBytes($file)
        $bytes[0] | Should -Be 137
        $bytes[1] | Should -Be 80
        $bytes[2] | Should -Be 78
        $bytes[3] | Should -Be 71
    }

    It 'can return the ChartForgeX topology model' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-pass-thru.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $topology = New-ImageTopology -TopologyDefinition {
            New-ImageTopologyNode -Id 'api' -Label 'API' -Kind Service -Status Healthy
        } -FilePath $file -NoTitle -PassThru

        $topology.Nodes.Count | Should -Be 1
        $topology.Nodes[0].Id | Should -Be 'api'
        Test-Path -Path $file | Should -BeTrue
    }

    It 'uses the canonical topology colors in either theme' -TestCases @(
        @{ Theme = 'Light' }
        @{ Theme = 'Dark' }
    ) {
        param($Theme)
        $tokens = if ($Theme -eq 'Dark') {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteDark()
        } else {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteLight()
        }
        $topology = New-ImageTopology -TopologyDefinition {
            New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy
        } -Theme $Theme -FilePath (Join-Path -Path $TestDir -ChildPath ("topology-theme-{0}.svg" -f $Theme)) -PassThru

        $topology.ThemeMode.ToString() | Should -BeExactly $Theme
        $topology.Theme.Resolve($topology.ThemeMode).Foreground.ToCss() | Should -BeExactly $tokens.Foreground.ToCss()
        $topology.Theme.Resolve($topology.ThemeMode).Positive.ToCss() | Should -BeExactly $tokens.Positive.ToCss()
        $topology.Theme.Typography.Family | Should -BeExactly ([ChartForgeX.Themes.VisualTheme]::Graphite()).Typography.Family
    }

    It 'accepts a direct topology chart without a Definition argument and preserves chart layout and custom theme' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-chart-input.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $chart = [ChartForgeX.Topology.TopologyChart]::Create()
        $chart.LayoutMode = [ChartForgeX.Topology.TopologyLayoutMode]::Manual
        $chart.LayoutDirection = [ChartForgeX.Topology.TopologyLayoutDirection]::RightToLeft
        $chart.Viewport.Width = 640
        $chart.Viewport.Height = 360
        $chart.Viewport.Padding = 12
        $chart.Theme = [ChartForgeX.Themes.VisualTheme]::Graphite().Configure({
            param($builder)
            $builder.Light.Foreground = [ChartForgeX.Primitives.ChartColor]::FromHex('#334455')
        })

        $node = [ChartForgeX.Topology.TopologyNode]::new()
        $node.Id = 'api'
        $node.Label = 'API'
        $chart.Nodes.Add($node)

        $topology = New-ImageTopology -Chart $chart -FilePath $file -NoTitle -PassThru

        $topology.LayoutMode | Should -Be ([ChartForgeX.Topology.TopologyLayoutMode]::Manual)
        $topology.LayoutDirection | Should -Be ([ChartForgeX.Topology.TopologyLayoutDirection]::RightToLeft)
        $topology.Viewport.Width | Should -Be 640
        $topology.Viewport.Height | Should -Be 360
        $topology.Viewport.Padding | Should -Be 12
        $topology.Theme.Resolve($topology.ThemeMode).Foreground.ToCss() | Should -BeExactly '#334455'
        Test-Path -Path $file | Should -BeTrue
    }

    It 'adds named inputs only once when pipeline input is also used' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-pipeline.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $nodes = @(
            New-ImageTopologyNode -Id 'api' -Label 'API'
            New-ImageTopologyNode -Id 'db' -Label 'Database'
        )
        $edges = @(
            New-ImageTopologyEdge -SourceNodeId 'api' -TargetNodeId 'db' -Label 'primary'
            New-ImageTopologyEdge -SourceNodeId 'db' -TargetNodeId 'api' -Label 'reply'
        )

        $topology = $edges | New-ImageTopology -Node $nodes -FilePath $file -NoTitle -PassThru

        $topology.Nodes.Count | Should -Be 2
        $topology.Edges.Count | Should -Be 2
        Test-Path -Path $file | Should -BeTrue
    }

    It 'rejects unsupported output extensions' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology.jpg'

        $script:topologyDefinitionInvoked = $false

        {
            New-ImageTopology -TopologyDefinition {
                $script:topologyDefinitionInvoked = $true
                New-ImageTopologyNode -Id 'api' -Label 'API'
            } -FilePath $file
        } | Should -Throw
        $script:topologyDefinitionInvoked | Should -BeFalse
    }

    It 'generates unique default edge identifiers for parallel edges' {
        $first = New-ImageTopologyEdge -SourceNodeId 'api' -TargetNodeId 'db'
        $second = New-ImageTopologyEdge -SourceNodeId 'api' -TargetNodeId 'db'

        $first.Id | Should -Not -Be $second.Id
        $first.Id | Should -Match '^api-db-\d+$'
        $second.Id | Should -Match '^api-db-\d+$'
    }

    It 'binds named ports, typed details, edge layout hints, and diagnostics' {
        $outbound = New-ImageTopologyNodePort -Id outbound -Side Right -Offset 0.7 -Label 'gRPC'
        $detail = New-ImageTopologyNodeDetail -Label Runtime -Value '.NET 10' -Status Healthy
        $api = New-ImageTopologyNode -Id api -Label API -Port $outbound -Detail $detail
        $db = New-ImageTopologyNode -Id db -Label Database -Port (New-ImageTopologyNodePort -Id inbound -Side Left)
        $edge = New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -SourcePortId outbound -TargetPortId inbound `
            -SourceMarker Circle -TargetMarker Arrow -StrokeWidth 2.5 -Opacity 0.75 -DashPattern 8, 4 `
            -PreferredLength 180 -MinimumRankSpan 2 -RoutingPriority 10 -SourceLabel gRPC -TargetLabel SQL
        $file = Join-Path $TestDir 'topology-advanced.svg'
        $chart = New-ImageTopology -Node $api, $db -Edge $edge -LayoutPreset Presentation -NoTitle -FilePath $file -PassThru

        $chart.Nodes[0].Ports[0].Id | Should -Be 'outbound'
        $chart.Nodes[0].Details[0].Value | Should -Be '.NET 10'
        $chart.Edges[0].PreferredLength | Should -Be 180
        $chart.Edges[0].MinimumRankSpan | Should -Be 2
        $chart.Edges[0].DashPattern | Should -Be @(8, 4)
        $diagnostics = $chart | Get-ImageTopologyDiagnostics
        $diagnostics.Nodes.Count | Should -Be 2
        $diagnostics.Edges.Count | Should -Be 1
    }

    It 'rejects non-positive dash pattern values' {
        { New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -DashPattern 8, -1 -ErrorAction Stop } |
            Should -Throw '*positive and finite*'
    }

    It 'treats explicit null topology arrays as empty collections' {
        $edge = New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -DashPattern $null
        $node = New-ImageTopologyNode -Id api -Label API -Port $null -Detail $null

        $edge.DashPattern.Count | Should -Be 0
        $node.Ports.Count | Should -Be 0
        $node.Details.Count | Should -Be 0
    }

    It 'still rejects null entries inside populated topology arrays' {
        $port = New-ImageTopologyNodePort -Id outbound -Side Right
        { New-ImageTopologyNode -Id api -Label API -Port @($port, $null) -ErrorAction Stop } |
            Should -Throw '*cannot contain null entries*'
    }

    It 'rejects interactive HTML with watermarks instead of silently producing static HTML' {
        $file = Join-Path $TestDir 'topology-interactive-watermark.html'
        $watermark = New-ImageVisualWatermark -Text INTERNAL

        {
            New-ImageTopology -Node (New-ImageTopologyNode -Id api -Label API) `
                -InteractiveHtml -Watermark $watermark -FilePath $file -ErrorAction Stop
        } | Should -Throw '*cannot currently be combined*'
    }

    It 'creates a nested output directory for watermarked charts' {
        $file = Join-Path $TestDir 'nested/chart/chart.svg'

        New-ImageChart -ChartsDefinition {
            New-ImageChartLine -Name API -Value 1, 2, 3
        } -Watermark (New-ImageVisualWatermark -Text INTERNAL) -FilePath $file

        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'retains active route preference while giving scenario controls an undimmed motion presentation' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-scenario.html'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $edge = New-ImageTopologyEdge -Id 'api-db' -SourceNodeId 'api' -TargetNodeId 'db' -Label Queries -Direction Forward
        $cacheEdge = New-ImageTopologyEdge -Id api-cache -SourceNodeId api -TargetNodeId cache -Label Lookups -Direction Forward
        $cacheScenario = New-ImageTopologyScenario -Id cache-flow -Label 'Cache flow' -StepDefinition {
            New-ImageTopologyScenarioStep -Id api-cache -Kind Edge
        } -Spotlight
        $scenario = New-ImageTopologyScenario -Id request -Label 'Request flow' -StepDefinition {
            New-ImageTopologyScenarioStep -Id api -Kind Node -Label 'API receives request'
            New-ImageTopologyScenarioStep -Id api-db -Kind Edge -Label 'Database call'
        } -AutoPlay -Spotlight
        $motion = New-ImageTopologyMotion -DurationSeconds 1 -FramesPerSecond 2 -MaximumRasterFrames 4 -NoLoop

        New-ImageTopology -Node @(
            New-ImageTopologyNode -Id api -Label API
            New-ImageTopologyNode -Id db -Label Database
            New-ImageTopologyNode -Id cache -Label Cache
        ) -Edge $edge, $cacheEdge -Scenario $cacheScenario, $scenario -Motion $motion -ActiveScenarioId request -InteractiveHtml -ScenarioUrlState -FilePath $file

        Test-Path -Path $file | Should -BeTrue
        $html = Get-Content -Path $file -Raw
        $html | Should -Match 'data-cfx-topology-scenario="request"'
        $html | Should -Match 'data-cfx-active-scenario="request"'
        $html | Should -Match 'animateMotion'
        $html | Should -Match 'data-cfx-motion-kind="route-pulse"'
        $html | Should -Match 'data-cfx-motion-source="request"'
        $embeddedSvg = [xml] (@([regex]::Matches($html, '(?s)<svg\b.*?</svg>') | Where-Object { $_.Value -match 'data-cfx-role="topology"' })[0].Value)
        $topologyRoot = $embeddedSvg.SelectSingleNode("//*[@data-cfx-role='topology']")
        $topologyRoot.GetAttribute('data-cfx-active-scenario') | Should -BeNullOrEmpty
        $cacheNode = $embeddedSvg.SelectSingleNode("//*[@data-cfx-role='topology-node' and @data-node-id='cache']")
        $cacheNode | Should -Not -BeNullOrEmpty
        $cacheNode.GetAttribute('class') | Should -Not -Match '--dimmed'
        $motion.ScenarioId | Should -BeNullOrEmpty
        $motion.EdgeIds.Count | Should -Be 0
    }

    It 'exports animated topology GIF and APNG files' {
        $gif = Join-Path -Path $TestDir -ChildPath 'topology-route.gif'
        $apng = Join-Path -Path $TestDir -ChildPath 'topology-route.apng'
        Remove-Item -Path $gif, $apng -ErrorAction SilentlyContinue
        $nodes = @(
            New-ImageTopologyNode -Id api -Label API
            New-ImageTopologyNode -Id db -Label Database
        )
        $edge = New-ImageTopologyEdge -Id 'api-db' -SourceNodeId api -TargetNodeId db -Direction Forward
        $motion = New-ImageTopologyMotion -EdgeId api-db -DurationSeconds 0.5 -FramesPerSecond 2 -MaximumRasterFrames 2 -NoLoop

        New-ImageTopology -Node $nodes -Edge $edge -Motion $motion -FilePath $gif -Width 320 -Height 200 -NoTitle
        New-ImageTopology -Node $nodes -Edge $edge -Motion $motion -FilePath $apng -Width 320 -Height 200 -NoTitle

        [System.Text.Encoding]::ASCII.GetString([System.IO.File]::ReadAllBytes($gif), 0, 3) | Should -Be 'GIF'
        $apngBytes = [System.IO.File]::ReadAllBytes($apng)
        $apngBytes[0] | Should -Be 137
        [System.Text.Encoding]::ASCII.GetString($apngBytes) | Should -Match 'acTL'
        $controlOffset = [System.Text.Encoding]::ASCII.GetString($apngBytes).IndexOf('acTL', [StringComparison]::Ordinal)
        $frameCount = $apngBytes[$controlOffset + 4] * 16777216 + $apngBytes[$controlOffset + 5] * 65536 + $apngBytes[$controlOffset + 6] * 256 + $apngBytes[$controlOffset + 7]
        $frameCount | Should -Be 1
        $motion.DurationSeconds | Should -Be 0.5
        $motion.FramesPerSecond | Should -Be 2
    }

    It 'preserves explicit topology motion and watermark decoration in script-free SVG and HTML exports' {
        $nodes = @(New-ImageTopologyNode -Id api -Label API; New-ImageTopologyNode -Id db -Label Database)
        $edge = New-ImageTopologyEdge -Id api-db -SourceNodeId api -TargetNodeId db
        $motion = New-ImageTopologyMotion -EdgeId api-db -DurationSeconds 1 -FramesPerSecond 2 -MaximumRasterFrames 4 -NoLoop
        $watermark = New-ImageVisualWatermark -Text INTERNAL -Anchor Center

        foreach ($extension in 'svg', 'html') {
            $file = Join-Path $TestDir "explicit-topology-motion.$extension"
            New-ImageTopology -Node $nodes -Edge $edge -Motion $motion -Watermark $watermark -Width 320 -Height 200 -NoTitle -FilePath $file
            $content = Get-Content -LiteralPath $file -Raw
            $content | Should -Match 'animateMotion'
            $content | Should -Match 'data-cfx-motion-kind="route-pulse"'
            $content | Should -Match 'INTERNAL'
            $content | Should -Not -Match '<script'
        }
    }

    It 'renders the selected topology motion progress sample in a fixed PNG with DPI metadata' {
        $nodes = @(New-ImageTopologyNode -Id api -Label API; New-ImageTopologyNode -Id db -Label Database)
        $edge = New-ImageTopologyEdge -Id api-db -SourceNodeId api -TargetNodeId db
        $motion = New-ImageTopologyMotion -EdgeId api-db -Progress 0.5 -MarkerColor '#FF0000'
        $file = Join-Path $TestDir 'topology-motion-sample.png'

        $chart = New-ImageTopology -Node $nodes -Edge $edge -Motion $motion -Width 320 -Height 200 -NoTitle -FitContentToViewport -Dpi 144 -FilePath $file -PassThru
        $options = [ChartForgeX.Topology.TopologyRenderOptions]::new()
        $options.IncludeTitle = $false
        $options.FitContentToViewport = $true
        $expected = [ChartForgeX.Topology.TopologyMotionExtensions]::WithMotion($chart, $motion, $options).ToRgbaImage()
        $encoded = [IO.File]::ReadAllBytes($file)
        $actual = [ChartForgeX.Raster.RasterImageDecoder]::Decode($encoded)

        $actual.Width | Should -Be 320
        $actual.Height | Should -Be 200
        [Convert]::ToBase64String($actual.Pixels) | Should -Be ([Convert]::ToBase64String($expected.Pixels))
        [Text.Encoding]::ASCII.GetString($encoded) | Should -Match 'pHYs'
        $motion.Progress | Should -Be 0.5
    }

    It 'lets automatic layouts place groups that have no explicit position' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-dense-groups.svg'
        $topology = New-ImageTopology -Layout DenseGrouped -Width 900 -Height 400 -FilePath $file -PassThru -TopologyDefinition {
            New-ImageTopologyGroup -Id g1 -Label One
            New-ImageTopologyGroup -Id g2 -Label Two
            New-ImageTopologyGroup -Id g3 -Label Three
            New-ImageTopologyNode -Id a -Label A -GroupId g1
            New-ImageTopologyNode -Id b -Label B -GroupId g2
            New-ImageTopologyNode -Id c -Label C -GroupId g3
        }

        $diagnostics = $topology | Get-ImageTopologyDiagnostics
        $positions = @($diagnostics.Groups | ForEach-Object { '{0:N0},{1:N0}' -f $_.Bounds.X, $_.Bounds.Y } | Sort-Object -Unique)
        $positions.Count | Should -Be 3
    }

    It 'keeps explicit group coordinates when supplied' {
        $group = New-ImageTopologyGroup -Id pinned -Label Pinned -X 40 -Y 60 -Width 300 -Height 200
        $auto = New-ImageTopologyGroup -Id auto -Label Auto

        $group.X | Should -Be 40
        $group.Width | Should -Be 300
        $auto.Width | Should -Be 0
    }

    It 'orders Layered columns with -Layer' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-layers.svg'
        $topology = New-ImageTopology -Layout Layered -Direction LeftToRight -Width 900 -Height 400 -FilePath $file -PassThru -TopologyDefinition {
            New-ImageTopologyNode -Id gateway -Label Gateway -Kind Service -Layer 0
            New-ImageTopologyNode -Id orders -Label Orders -Kind Service -Layer 1
            New-ImageTopologyNode -Id sql -Label SQL -Kind Database -Layer 2
            New-ImageTopologyEdge -SourceNodeId gateway -TargetNodeId orders -Direction Forward
            New-ImageTopologyEdge -SourceNodeId orders -TargetNodeId sql -Direction Forward
        }

        $topology.Nodes[2].Metadata['layer'] | Should -Be '2'
        $nodes = @(($topology | Get-ImageTopologyDiagnostics).Nodes)
        $x = @{}
        foreach ($node in $nodes) { $x[$node.Id] = $node.Bounds.X }
        $x['gateway'] | Should -BeLessThan $x['orders']
        $x['orders'] | Should -BeLessThan $x['sql']
    }
}
