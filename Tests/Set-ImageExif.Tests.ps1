Describe 'Set-ImageExif' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'sets and removes exif data' {

        $dest = Join-Path $TestDir 'exif-edit.jpg'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')

        $img.Save()

        $img.Dispose()

        Set-ImageExif -FilePath $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Value 'Modified'

        (Get-ImageExif -FilePath $dest -Translate).Software | Should -Be 'Modified'

        Remove-ImageExif -FilePath $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Verbose

        (Get-ImageExif -FilePath $dest).Count | Should -Be 0

    }

    It 'sets exif in non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'exif-edit.jpg'
        Set-ImageExif -FilePath $src -FilePathOutput $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Value 'Modified'
        Test-Path $dest | Should -BeTrue
    }

    It 'throws when value type mismatches tag' {

        $dest = Join-Path $TestDir 'exif-type-error.jpg'
        if (Test-Path $dest) { Remove-Item $dest }

        $img = [ImagePlayground.Image]::new()
        $img.Create($dest, 10, 10)
        $img.Save()
        $img.Dispose()

        {
            Set-ImageExif -FilePath $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Value 123
        } | Should -Throw

    }

}

