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
}
