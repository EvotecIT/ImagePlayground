Describe 'New-ImageChartBubble' {
    BeforeAll {
        $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) { $env:IMAGEPLAYGROUND_TEST_MODULE_PATH } else { "$PSScriptRoot/../ImagePlayground.psd1" }
        Import-Module -Name $modulePath -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'creates a bubble chart' {
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
