Describe 'Set-ImageRotation' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'rotates an image by degrees' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dest = Join-Path $TestDir 'rotate-deg.jpg'
        Set-ImageRotation -FilePath $src -OutputPath $dest -Degrees 90
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Height
        $img.Height | Should -Be $orig.Width
        $img.Dispose()
        $orig.Dispose()
    }

    It 'rotates an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dest = Join-Path $TestDir 'rotate-async.jpg'
        Set-ImageRotation -FilePath $src -OutputPath $dest -Degrees 270 -Async
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Height
        $img.Height | Should -Be $orig.Width
        $img.Dispose()
        $orig.Dispose()
    }

    It 'rotates using rotate mode' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dest = Join-Path $TestDir 'rotate-mode.jpg'
        Set-ImageRotation -FilePath $src -OutputPath $dest -RotateMode Rotate270
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Height
        $img.Height | Should -Be $orig.Width
        $img.Dispose()
        $orig.Dispose()
    }

    It 'rotates into non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'rotate.jpg'
        Set-ImageRotation -FilePath $src -OutputPath $dest -Degrees 90
        Test-Path $dest | Should -BeTrue
    }
}
