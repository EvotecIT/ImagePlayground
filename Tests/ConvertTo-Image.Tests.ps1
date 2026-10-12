Describe 'ConvertTo-Image' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'converts image format' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'qr.jpg'
        if (Test-Path $dest) { Remove-Item $dest }
        ConvertTo-Image -FilePath $src -OutputPath $dest -Quality 80
        Test-Path $dest | Should -BeTrue
    }

    It 'copies icon when converting icon to icon' {
        $src = Join-Path $TestDir 'actual-icon-source.ico'
        $image = [ImagePlayground.Image]::FromRaster([OfficeIMO.Drawing.OfficeRasterImage]::new(16, 16, [OfficeIMO.Drawing.OfficeColor]::Blue))
        try {
            $image.Save($src)
        } finally {
            $image.Dispose()
        }
        $dest = Join-Path $TestDir 'qr_copy.ico'
        if (Test-Path $dest) { Remove-Item $dest }
        ConvertTo-Image -FilePath $src -OutputPath $dest
        Test-Path $dest | Should -BeTrue
        [Convert]::ToBase64String([IO.File]::ReadAllBytes($dest)) | Should -BeExactly ([Convert]::ToBase64String([IO.File]::ReadAllBytes($src)))
    }

    It 'rejects an oversized image without creating an invalid icon' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'invalid.ico'
        { ConvertTo-Image -FilePath $src -OutputPath $dest } | Should -Throw
        Test-Path -LiteralPath $dest | Should -BeFalse
    }

    It 'converts a valid image to an icon with the same dimensions and pixels' {
        $src = Join-Path $TestDir 'icon-input.png'
        $dest = Join-Path $TestDir 'valid.ico'
        $image = [ImagePlayground.Image]::FromRaster([OfficeIMO.Drawing.OfficeRasterImage]::new(16, 9, [OfficeIMO.Drawing.OfficeColor]::Red))
        try {
            $image.Save($src)
        } finally {
            $image.Dispose()
        }
        ConvertTo-Image -FilePath $src -OutputPath $dest
        $converted = [ImagePlayground.Image]::Load($dest)
        try {
            $converted.Width | Should -Be 16
            $converted.Height | Should -Be 9
            $converted.Raster.GetPixel(0, 0) | Should -Be ([OfficeIMO.Drawing.OfficeColor]::Red)
            $converted.Raster.GetPixel(15, 8) | Should -Be ([OfficeIMO.Drawing.OfficeColor]::Red)
        } finally {
            $converted.Dispose()
        }
    }
}
