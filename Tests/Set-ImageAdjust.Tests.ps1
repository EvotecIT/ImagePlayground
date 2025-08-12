Describe 'Set-ImageAdjust' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'adjusts an image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-adjust.png'
        Set-ImageAdjust -FilePath $src -OutputPath $dest -Brightness 1.2 -Contrast 1.1
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'adjusts an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-adjust-async.png'
        Set-ImageAdjust -FilePath $src -OutputPath $dest -Brightness 1.2 -Contrast 1.1 -Async
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }
}