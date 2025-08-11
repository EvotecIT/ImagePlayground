Describe 'Sharpen-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'sharpens an image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dest = Join-Path $TestDir 'sharpen.jpg'
        Sharpen-Image -FilePath $src -OutputPath $dest -Sigma 5
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }

    It 'sharpens an image asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/PrzemyslawKlysAndKulkozaurr.jpg'
        $dest = Join-Path $TestDir 'sharpen-async.jpg'
        Sharpen-Image -FilePath $src -OutputPath $dest -Sigma 3 -Async
        $orig = [ImagePlayground.Image]::Load($src)
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be $orig.Width
        $img.Height | Should -Be $orig.Height
        $img.Dispose()
        $orig.Dispose()
    }
}
