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

        Remove-ImageExif -FilePath $dest -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Verbose

        (Get-ImageExif -FilePath $dest).Count | Should -Be 0

    }

    It 'creates parent directory when output path provided' {

        $dest = Join-Path $TestDir 'exif-source.jpg'
        $output = Join-Path $TestDir 'nested/exif-output.jpg'

        if (Test-Path $dest) { Remove-Item $dest }
        if (Test-Path (Split-Path $output)) { Remove-Item (Split-Path $output) -Recurse }

        $img = [ImagePlayground.Image]::new()

        $img.Create($dest, 10, 10)

        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')

        $img.Save()

        $img.Dispose()

        Remove-ImageExif -FilePath $dest -FilePathOutput $output -ExifTag ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software) -Verbose

        Test-Path $output | Should -BeTrue
        (Get-ImageExif -FilePath $output).Count | Should -Be 0

    }

}

