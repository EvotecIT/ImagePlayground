Describe 'New-ImageTopology' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
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

        {
            New-ImageTopology -TopologyDefinition {
                New-ImageTopologyNode -Id 'api' -Label 'API'
            } -FilePath $file
        } | Should -Throw
    }

    It 'generates unique default edge identifiers for parallel edges' {
        $first = New-ImageTopologyEdge -SourceNodeId 'api' -TargetNodeId 'db'
        $second = New-ImageTopologyEdge -SourceNodeId 'api' -TargetNodeId 'db'

        $first.Id | Should -Not -Be $second.Id
        $first.Id | Should -Match '^api-db-\d+$'
        $second.Id | Should -Match '^api-db-\d+$'
    }
}
