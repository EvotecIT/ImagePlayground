Describe 'Get-ImageExif' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'reads exif data' {

        $dest = Join-Path $TestDir 'exif-read.jpg'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')

        $img.Save()

        $img.Dispose()

        $result = Get-ImageExif -FilePath $dest -Translate

        $result.Software | Should -Be 'ImagePlayground'

    }

}

