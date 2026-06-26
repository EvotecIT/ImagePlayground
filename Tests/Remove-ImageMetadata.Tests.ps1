Describe 'Remove-ImageMetadata' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'saves an image copy without metadata' {
        $source = Join-Path $TestDir 'metadata-source.jpg'
        $output = Join-Path $TestDir 'metadata-clean.jpg'

        if (Test-Path $source) { Remove-Item $source }
        if (Test-Path $output) { Remove-Item $output }

        $img = [ImagePlayground.Image]::new()
        $img.Create($source, 10, 10)
        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')
        $img.Save()
        $img.Dispose()

        Remove-ImageMetadata -FilePath $source -OutputPath $output

        Test-Path $output | Should -BeTrue
        (Get-ImageExif -FilePath $output).Count | Should -Be 0
    }
}
