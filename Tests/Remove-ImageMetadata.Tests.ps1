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

    It 'supports WhatIf without creating the output' {
        $source = Join-Path $TestDir 'metadata-whatif-source.jpg'
        $output = Join-Path $TestDir 'metadata-whatif-clean.jpg'

        if (Test-Path $source) { Remove-Item $source }
        if (Test-Path $output) { Remove-Item $output }

        $img = [ImagePlayground.Image]::new()
        $img.Create($source, 10, 10)
        $img.Save()
        $img.Dispose()

        Remove-ImageMetadata -FilePath $source -OutputPath $output -WhatIf

        Test-Path $output | Should -BeFalse
    }

    It 'removes only selected metadata and reports the result with PassThru' {
        $source = Join-Path $TestDir 'metadata-selected-source.jpg'
        $output = Join-Path $TestDir 'metadata-selected-clean.jpg'
        $metadataPath = Join-Path $TestDir 'metadata-selected.json'

        if (Test-Path $source) { Remove-Item $source }
        if (Test-Path $output) { Remove-Item $output }
        if (Test-Path $metadataPath) { Remove-Item $metadataPath }

        $img = [ImagePlayground.Image]::new()
        $img.Create($source, 10, 10)
        $img.SetExifValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::Software, 'ImagePlayground')
        $img.Save()
        $img.Dispose()

        $metadata = Export-ImageMetadata -FilePath $source | ConvertFrom-Json
        $metadata.XmpProfile = [Convert]::ToBase64String(
            [Text.Encoding]::UTF8.GetBytes('<x:xmpmeta xmlns:x="adobe:ns:meta/" />'))
        [IO.File]::WriteAllText($metadataPath, ($metadata | ConvertTo-Json -Depth 5))
        Import-ImageMetadata -FilePath $source -MetadataPath $metadataPath

        $result = Remove-ImageMetadata -FilePath $source -OutputPath $output -MetadataType Exif -PassThru
        $metadata = Get-ImageMetadata -FilePath $output

        $result.RemovedMetadataTypes | Should -Be ([ImagePlayground.ImageMetadataType]::Exif)
        $result.WasReencoded | Should -BeFalse
        $metadata.HasExif | Should -BeFalse
        $metadata.HasXmp | Should -BeTrue
    }

    It 'rejects conflicting All and MetadataType selections' {
        $source = Join-Path $TestDir 'metadata-conflict-source.jpg'
        $output = Join-Path $TestDir 'metadata-conflict-clean.jpg'

        if (Test-Path $source) { Remove-Item $source }
        if (Test-Path $output) { Remove-Item $output }

        $img = [ImagePlayground.Image]::new()
        $img.Create($source, 10, 10)
        $img.Save()
        $img.Dispose()

        { Remove-ImageMetadata -FilePath $source -OutputPath $output -All -MetadataType Xmp -ErrorAction Stop } | Should -Throw
        Test-Path $output | Should -BeFalse
    }
}
