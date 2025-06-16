Describe 'New-ImageChart' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates a bar chart' -Skip:(-not $IsWindows) {
        $file = Join-Path $TestDir 'chart.png'
        if (Test-Path $file) { Remove-Item $file }

        New-ImageChart -ChartsDefinition {
            New-ImageChartBar -Name 'Jan' -Value @(1,2)
            New-ImageChartBar -Name 'Feb' -Value @(3,4)
        } -FilePath $file -Width 200 -Height 150

        Test-Path $file | Should -BeTrue
    }
}
