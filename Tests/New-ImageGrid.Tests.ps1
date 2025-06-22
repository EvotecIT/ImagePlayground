Describe 'New-ImageGrid' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'creates a grid image' {

        $dest = Join-Path $TestDir 'grid.png'

        if (Test-Path $dest) { Remove-Item $dest }

        New-ImageGrid -FilePath $dest -Width 50 -Height 50

        Test-Path $dest | Should -BeTrue

    }

    It 'throws for negative dimensions' {

        $dest = Join-Path $TestDir 'grid.png'

        { New-ImageGrid -FilePath $dest -Width -5 -Height 10 } | Should -Throw
        { New-ImageGrid -FilePath $dest -Width 10 -Height -5 } | Should -Throw

    }

}

