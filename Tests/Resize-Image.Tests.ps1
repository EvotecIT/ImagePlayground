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

    It 'resizes an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-small-async.png'

        Resize-Image -FilePath $src -OutputPath $dest -Width 40 -Height 40 -Async

        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be 40
        $img.Height | Should -Be 40
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
    It 'throws for invalid percentage' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-invalid.png'
        { Resize-Image -FilePath $src -OutputPath $dest -Percentage 0 } | Should -Throw
        { Resize-Image -FilePath $src -OutputPath $dest -Percentage -5 } | Should -Throw
    }

    It 'throws for negative dimensions' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-invalid-dim.png'
        { Resize-Image -FilePath $src -OutputPath $dest -Width -5 -Height 10 } | Should -Throw
        { Resize-Image -FilePath $src -OutputPath $dest -Width 10 -Height -5 } | Should -Throw
    }

    It 'throws for dimensions above maximum' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-too-large.png'
        { Resize-Image -FilePath $src -OutputPath $dest -Width 1200 -Height 100 } | Should -Throw
        { Resize-Image -FilePath $src -OutputPath $dest -Width 100 -Height 1200 } | Should -Throw
    }
    It 'accepts maximum dimension values' {
        $src = Join-Path $PSScriptRoot "../Sources/ImagePlayground.Tests/Images/LogoEvotec.png"
        $dest = Join-Path $TestDir "logo-max.png"
        { Resize-Image -FilePath $src -OutputPath $dest -Width 1000 -Height 1000 } | Should -Not -Throw
    }


    It 'throws when DontRespectAspectRatio without dimensions' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-invalid-aspect.png'
        { Resize-Image -FilePath $src -OutputPath $dest -DontRespectAspectRatio } | Should -Throw
    }


}

