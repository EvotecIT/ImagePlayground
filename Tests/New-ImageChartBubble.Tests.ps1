Describe 'New-ImageChartBubble' {
    BeforeAll {
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'creates a bubble chart' -Skip:(-not $IsWindows) {
        $file = Join-Path -Path $TestDir -ChildPath 'chart_bubble.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBubble -Name 'S1' -X @(1,2) -Y @(3,4) -Size @(5,10)
        } -FilePath $file -Width 200 -Height 150

        Test-Path -Path $file | Should -BeTrue
    }
}
