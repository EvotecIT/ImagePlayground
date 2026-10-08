Describe 'Remove-ImageExif' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'removes the selected EXIF tag and preserves other tags' {

        $dest = Join-Path $TestDir 'exif-remove.jpg'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::Software, 'ImagePlayground')
        $img.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::Artist, 'Retained artist')

        $img.Save()

        $img.Dispose()

        Remove-ImageExif -FilePath $dest -ExifTag ([OfficeIMO.Drawing.OfficeExifTag]::Software) -Verbose

        $remaining = Get-ImageExif -FilePath $dest -Translate
        $remaining.Software | Should -BeNullOrEmpty
        $remaining.Artist | Should -Be 'Retained artist'

    }

    It 'creates parent directory when output path provided' {

        $dest = Join-Path $TestDir 'exif-source.jpg'
        $output = Join-Path $TestDir 'nested/exif-output.jpg'

        if (Test-Path $dest) { Remove-Item $dest }
        if (Test-Path (Split-Path $output)) { Remove-Item (Split-Path $output) -Recurse }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::Software, 'ImagePlayground')

        $img.Save()

        $img.Dispose()

        Remove-ImageExif -FilePath $dest -FilePathOutput $output -ExifTag ([OfficeIMO.Drawing.OfficeExifTag]::Software) -Verbose

        Test-Path $output | Should -BeTrue
        (Get-ImageExif -FilePath $output -Translate).Software | Should -BeNullOrEmpty
        (Get-ImageExif -FilePath $dest -Translate).Software | Should -Be 'ImagePlayground'

    }

}

