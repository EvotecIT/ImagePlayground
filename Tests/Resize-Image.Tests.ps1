Describe 'Resize-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'resizes an image' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        $dest = Join-Path $TestDir 'logo-small.png'

        Resize-Image -FilePath $src -OutputPath $dest -Width 50 -Height 50

        $img = [ImagePlayground.Image]::Load($dest)

        $img.Width | Should -Be 50

        $img.Height | Should -Be 50

        $img.Dispose()

    }

    It 'resizes an image by percentage' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        $dest = Join-Path $TestDir 'logo-percent.png'

        Resize-Image -FilePath $src -OutputPath $dest -Percentage 50

        $original = [ImagePlayground.Image]::Load($src)

        $img = [ImagePlayground.Image]::Load($dest)

        $expectedWidth = [math]::Floor($original.Width * 0.5)

        $expectedHeight = [math]::Floor($original.Height * 0.5)

        $img.Width | Should -Be $expectedWidth

        $img.Height | Should -Be $expectedHeight

        $img.Dispose()

        $original.Dispose()

    }

}

