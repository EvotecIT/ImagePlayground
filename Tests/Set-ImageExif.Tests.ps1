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

