Describe 'Save-Image' {

    BeforeAll {

        $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            $env:IMAGEPLAYGROUND_TEST_MODULE_PATH
        } else {
            Join-Path $PSScriptRoot '../ImagePlayground.psd1'
        }
        Import-Module -Name $modulePath -Force -ErrorAction Stop



        $TestDir = Join-Path $TestDrive 'Artifacts'

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

    It 'reports a missing destination for an image created from pixels without writing output' {
        $img = [ImagePlayground.Image]::FromRaster([OfficeIMO.Drawing.OfficeRasterImage]::new(4, 3))
        $filesBefore = @(Get-ChildItem -LiteralPath $TestDir -File -Recurse).Count
        try {
            $failure = try { $img | Save-Image -ErrorAction Stop } catch { $_ }
            $failure.FullyQualifiedErrorId | Should -Match '^SaveImageMissingPath,'
            $failure.CategoryInfo.Category | Should -Be ([System.Management.Automation.ErrorCategory]::InvalidArgument)
            $failure.Exception | Should -BeOfType ([System.Management.Automation.PSArgumentException])
            $failure.Exception.Message | Should -Match 'FilePath is required'
            @(Get-ChildItem -LiteralPath $TestDir -File -Recurse).Count | Should -Be $filesBefore
            $img.FilePath | Should -BeNullOrEmpty
            $img.Width | Should -Be 4
            $img.Height | Should -Be 3
        } finally {
            $img.Dispose()
        }
    }

    It 'saves pixel-created images to explicit destinations and pathless streams' {
        $img = [ImagePlayground.Image]::FromRaster([OfficeIMO.Drawing.OfficeRasterImage]::new(4, 3))
        $dest = Join-Path -Path $TestDir -ChildPath 'pixel-image.png'
        $stream = $null
        $saved = $null
        try {
            Save-Image -Image $img -FilePath $dest
            $saved = Get-Image -FilePath $dest
            $saved.Width | Should -Be 4
            $saved.Height | Should -Be 3
            $stream = Save-Image -Image $img -AsStream
            $stream | Should -BeOfType ([System.IO.MemoryStream])
            $stream.Position | Should -Be 0
            [Convert]::ToBase64String($stream.ToArray()) | Should -BeExactly ([Convert]::ToBase64String([System.IO.File]::ReadAllBytes($dest)))
        } finally {
            if ($saved) { $saved.Dispose() }
            if ($stream) { $stream.Dispose() }
            $img.Dispose()
        }
    }

    It 'uses the associated path when FilePath is omitted' {
        $dest = Join-Path -Path $TestDir -ChildPath 'associated-image.png'
        $img = [ImagePlayground.Image]::new()
        try {
            $img.Create($dest, 4, 3)
            $img | Save-Image
            [System.IO.File]::Exists($dest) | Should -BeTrue
            $img.FilePath | Should -Be $dest
        } finally {
            $img.Dispose()
        }
    }
}
