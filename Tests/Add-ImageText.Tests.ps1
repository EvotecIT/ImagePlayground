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

    It 'adds text with shadow and outline' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'text_shadow_outline.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red) -ShadowColor ([SixLabors.ImageSharp.Color]::Black) -ShadowOffsetX 1 -ShadowOffsetY 1 -OutlineColor ([SixLabors.ImageSharp.Color]::Yellow) -OutlineWidth 1

        Test-Path $dest | Should -BeTrue

    }

}

