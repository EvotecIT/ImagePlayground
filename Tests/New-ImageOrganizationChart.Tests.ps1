Describe 'New-ImageOrganizationChart' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'renders a compact organization hierarchy and returns the native topology model' {
        $file = Join-Path -Path $TestDir -ChildPath 'organization.svg'
        Remove-Item -Path $file -ErrorAction SilentlyContinue

        $chart = New-ImageOrganizationChart -TeamId engineering -TeamLabel Engineering -LayoutPolicy Auto -MemberDefinition {
            New-ImageOrganizationMember -Id director -Name 'Avery Stone' -Role Director -Status Healthy -LayoutPolicy Standard
            New-ImageOrganizationMember -Id platform -Name 'Morgan Lee' -Role 'Platform Lead' -ParentId director -Status Healthy -LayoutPolicy Compact
            New-ImageOrganizationMember -Id runtime -Name 'Sam Park' -Role 'Runtime Lead' -ParentId director -Status Warning -LayoutPolicy Vertical
            New-ImageOrganizationMember -Id engineer -Name 'Alex Kim' -Role Engineer -ParentId platform -Status Healthy
            New-ImageOrganizationMember -Id analyst -Name 'Chris Gray' -Role Analyst -ParentId runtime -Status Healthy
        } -FilePath $file -Width 900 -Height 520 -PassThru

        $chart | Should -BeOfType 'ChartForgeX.Topology.TopologyChart'
        $chart.Nodes.Count | Should -Be 6
        $chart.Edges.Count | Should -Be 5
        $chart.LayoutMode | Should -Be ([ChartForgeX.Topology.TopologyLayoutMode]::Layered)
        ($chart.Nodes | Where-Object Id -eq director).Metadata['hierarchy.layoutPolicy'] | Should -Be 'Standard'
        ($chart.Nodes | Where-Object Id -eq platform).Metadata['hierarchy.layoutPolicy'] | Should -Be 'Compact'
        ($chart.Nodes | Where-Object Id -eq runtime).Metadata['hierarchy.layoutPolicy'] | Should -Be 'Vertical'
        ($chart.Nodes | Where-Object Id -eq engineer).Metadata['hierarchy.layoutPolicy'] | Should -Be 'Compact'
        ($chart.Nodes | Where-Object Id -eq analyst).Metadata['hierarchy.layoutPolicy'] | Should -Be 'Vertical'
        Test-Path -Path $file | Should -BeTrue
        $svg = Get-Content -Path $file -Raw
        $svg | Should -Match 'Avery Stone'
        $svg | Should -Match 'hierarchy.relationship'
        $svg | Should -Match 'layout-hierarchypolicy="Compact"'
        $svg | Should -Match 'layout-hierarchypolicy="Vertical"'
    }

    It 'accepts organization members from the pipeline' {
        $file = Join-Path -Path $TestDir -ChildPath 'organization-pipeline.png'
        Remove-Item -Path $file -ErrorAction SilentlyContinue
        $members = @(
            New-ImageOrganizationMember -Id lead -Name Lead
            New-ImageOrganizationMember -Id member -Name Member -ParentId lead
        )

        $members | New-ImageOrganizationChart -TeamLabel Team -NoTeamNode -FilePath $file -Width 480 -Height 300

        Test-Path -Path $file | Should -BeTrue
        $bytes = [System.IO.File]::ReadAllBytes($file)
        $bytes[0] | Should -Be 137
        $bytes[1] | Should -Be 80
    }

    It 'keeps member policy optional so chart defaults can flow through' {
        $explicit = New-ImageOrganizationMember -Id explicit -Name Explicit -LayoutPolicy Compact
        $inherited = New-ImageOrganizationMember -Id inherited -Name Inherited

        $explicit.LayoutPolicy | Should -Be ([ChartForgeX.Topology.TopologyHierarchyLayoutPolicy]::Compact)
        $inherited.LayoutPolicy | Should -BeNullOrEmpty
    }
}
