Describe 'Remove-ImageExif' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'removes exif data' {

        $dest = Join-Path $TestDir 'exif-remove.jpg'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')

        $img.Save()

        $img.Dispose()

        Remove-ImageExif -FilePath $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software)

        (Get-ImageExif -FilePath $dest).Count | Should -Be 0

    }

}

