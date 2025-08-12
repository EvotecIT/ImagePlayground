Describe 'New-ImageMosaic' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'creates a mosaic image' {

        $src1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $src2 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $src3 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN7.png'
        $src4 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN13.png'
        $dest = Join-Path $TestDir 'mosaic.png'

        if (Test-Path $dest) { Remove-Item $dest }

        New-ImageMosaic -FilePaths @($src1,$src2,$src3,$src4) -OutputPath $dest -Columns 2 -Width 100 -Height 100

        Test-Path $dest | Should -BeTrue

        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be 200
        $img.Height | Should -Be 200
        $img.Dispose()

    }

    It 'creates mosaic in non-existent directory' {
        $src1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $src2 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $src3 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN7.png'
        $src4 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN13.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'mosaic.png'
        New-ImageMosaic -FilePaths @($src1,$src2,$src3,$src4) -OutputPath $dest -Columns 2 -Width 100 -Height 100
        Test-Path $dest | Should -BeTrue
    }

}
