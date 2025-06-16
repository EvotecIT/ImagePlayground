Describe 'Add-ImageText' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'adds text to an image' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'text.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)

        Test-Path $dest | Should -BeTrue

    }

}

