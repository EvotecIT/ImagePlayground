Describe 'Set-ImageBlur' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'blurs an image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-blur.png'
        Set-ImageBlur -FilePath $src -OutputPath $dest -Amount 5
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'blurs an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
       $dest = Join-Path $TestDir 'logo-blur-async.png'
        Set-ImageBlur -FilePath $src -OutputPath $dest -Amount 5 -Async
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'blurs to non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'logo-blur.png'
        Set-ImageBlur -FilePath $src -OutputPath $dest -Amount 5
        Test-Path $dest | Should -BeTrue
    }
}
