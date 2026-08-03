Describe 'Get-ImageMetadata' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
    }

    It 'returns a unified metadata snapshot' {
        $source = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $result = Get-ImageMetadata -FilePath $source

        $result.FilePath | Should -Be ([System.IO.Path]::GetFullPath($source))
        $result.HasExif | Should -BeFalse
        $result.HasXmp | Should -BeFalse
        $result.Provenance.HasC2paManifest | Should -BeFalse
        $result.Provenance.C2paValidationPerformed | Should -BeFalse
    }

    It 'accepts FilePath from the pipeline' {
        $source = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $result = $source | Get-ImageMetadata

        $result.FilePath | Should -Be ([System.IO.Path]::GetFullPath($source))
    }

    It 'throws when the file does not exist' {
        $missing = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/missing.png'

        { Get-ImageMetadata -FilePath $missing -ErrorAction Stop } | Should -Throw
    }
}
