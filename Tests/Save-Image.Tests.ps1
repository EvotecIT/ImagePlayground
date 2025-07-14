Describe 'Save-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'saves an image to a new location' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'saved.png'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = Get-Image -FilePath $src

        Save-Image -Image $img -FilePath $dest -CompressionLevel 6 -Quality 80

        $img.Dispose()

        Test-Path $dest | Should -BeTrue

    }

    It 'returns a MemoryStream when AsStream is used' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $img = Get-Image -FilePath $src

        $stream = Save-Image -Image $img -AsStream
        
        $img.Dispose()

        $stream | Should -BeOfType ([System.IO.MemoryStream])

        $stream.Length | Should -BeGreaterThan 0

    }

    It 'supports quality and compression when AsStream is used' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $img = Get-Image -FilePath $src

        $stream = Save-Image -Image $img -AsStream -Quality 80 -CompressionLevel 6

        $img.Dispose()

        $stream | Should -BeOfType ([System.IO.MemoryStream])
        $stream.Length | Should -BeGreaterThan 0
    }

    It 'creates directory when saving to a new folder' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $folder = Join-Path $TestDir 'NewFolder'
        $dest = Join-Path $folder 'saved.png'
        if (Test-Path $folder) { Remove-Item $folder -Recurse -Force }
        $img = Get-Image -FilePath $src
        Save-Image -Image $img -FilePath $dest
        $img.Dispose()
        Test-Path $dest | Should -BeTrue
    }
}
