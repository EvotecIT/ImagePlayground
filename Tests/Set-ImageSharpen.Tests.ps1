Describe 'Set-ImageSharpen' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'sharpens an image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-sharp.png'
        Set-ImageSharpen -FilePath $src -OutputPath $dest -Amount 2
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'sharpens an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-sharp-async.png'
        Set-ImageSharpen -FilePath $src -OutputPath $dest -Amount 2 -Async
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'sharpens into non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'logo-sharp.png'
        Set-ImageSharpen -FilePath $src -OutputPath $dest -Amount 2
        Test-Path $dest | Should -BeTrue
    }
}
