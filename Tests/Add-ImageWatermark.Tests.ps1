Describe 'Add-ImageWatermark' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'adds watermark without resizing' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'watermark.png'
        $wmk = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        if (Test-Path -Path $dest) { Remove-Item $dest }

        Add-ImageWatermark -FilePath $src -OutputPath $dest -WatermarkPath $wmk -WatermarkPercentage 100
        Test-Path -Path $dest | Should -BeTrue
        $out = [ImagePlayground.Image]::Load($dest)
        $orig = [ImagePlayground.Image]::Load($src)
        $out.Width | Should -Be $orig.Width
        $out.Height | Should -Be $orig.Height
        $out.Dispose()
        $orig.Dispose()
    }

    It 'adds watermark with placement parameter' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'watermark-placement.png'
        $wmk = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        if (Test-Path -Path $dest) { Remove-Item $dest }

        Add-ImageWatermark -FilePath $src -OutputPath $dest -WatermarkPath $wmk -Placement ([ImagePlayground.WatermarkPlacement]::TopLeft) -WatermarkPercentage 100
        Test-Path -Path $dest | Should -BeTrue
        $out = [ImagePlayground.Image]::Load($dest)
        $orig = [ImagePlayground.Image]::Load($src)
        $out.Width | Should -Be $orig.Width
        $out.Height | Should -Be $orig.Height
        $out.Dispose()
        $orig.Dispose()
    }

    It 'adds watermark asynchronously' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'watermark-async.png'
        $wmk = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        if (Test-Path -Path $dest) { Remove-Item $dest }

        Add-ImageWatermark -FilePath $src -OutputPath $dest -WatermarkPath $wmk -WatermarkPercentage 100 -Async
        Test-Path -Path $dest | Should -BeTrue
        $out = [ImagePlayground.Image]::Load($dest)
        $orig = [ImagePlayground.Image]::Load($src)
        $out.Width | Should -Be $orig.Width
        $out.Height | Should -Be $orig.Height
        $out.Dispose()
        $orig.Dispose()
    }

    It 'creates directory when adding watermark to a new folder' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $wmk = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $folder = Join-Path $TestDir 'WatermarkFolder'
        $dest = Join-Path $folder 'watermark.png'
        if (Test-Path -Path $folder) { Remove-Item $folder -Recurse -Force }

        Add-ImageWatermark -FilePath $src -OutputPath $dest -WatermarkPath $wmk -WatermarkPercentage 100

        Test-Path -Path $dest | Should -BeTrue
    }
}
