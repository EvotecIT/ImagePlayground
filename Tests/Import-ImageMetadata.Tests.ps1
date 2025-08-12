describe 'Import-ImageMetadata' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }
    It 'imports metadata to non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $meta = Join-Path $TestDir 'meta.json'
        Export-ImageMetadata -FilePath $src -OutputPath $meta
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'logo.png'
        Import-ImageMetadata -FilePath $src -MetadataPath $meta -OutputPath $dest
        Test-Path $dest | Should -BeTrue
    }
}
