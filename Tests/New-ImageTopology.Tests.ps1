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

    It 'accepts a direct topology chart without a Definition argument and preserves chart layout' {
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

    It 'renders scenario controls and script-free route motion' {
        $file = Join-Path -Path $TestDir -ChildPath 'topology-scenario.html'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $edge = New-ImageTopologyEdge -Id 'api-db' -SourceNodeId 'api' -TargetNodeId 'db' -Direction Forward
        $scenario = New-ImageTopologyScenario -Id request -Label 'Request flow' -StepDefinition {
            New-ImageTopologyScenarioStep -Id api -Kind Node -Label 'API receives request'
            New-ImageTopologyScenarioStep -Id api-db -Kind Edge -Label 'Database call'
        } -AutoPlay -Spotlight
        $motion = New-ImageTopologyMotion -ScenarioId request -DurationSeconds 1 -FramesPerSecond 2 -MaximumRasterFrames 4 -NoLoop

        New-ImageTopology -Node @(
            New-ImageTopologyNode -Id api -Label API
            New-ImageTopologyNode -Id db -Label Database
        ) -Edge $edge -Scenario $scenario -Motion $motion -ActiveScenarioId request -InteractiveHtml -ScenarioUrlState -FilePath $file

        Test-Path -Path $file | Should -BeTrue
        $html = Get-Content -Path $file -Raw
        $html | Should -Match 'data-cfx-topology-scenario="request"'
        $html | Should -Match 'data-cfx-active-scenario="request"'
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
    }
}
