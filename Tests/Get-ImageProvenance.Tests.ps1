Describe 'Get-ImageProvenance' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
    }

    It 'returns provenance state for a clean image' {
        $source = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $result = Get-ImageProvenance -FilePath $source

        $result.HasC2paManifest | Should -BeFalse
        $result.HasXmpAiDeclaration | Should -BeFalse
        $result.C2paValidationPerformed | Should -BeFalse
        $result.Evidence.Count | Should -Be 0
    }

    It 'accepts FilePath from the pipeline' {
        $source = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $result = $source | Get-ImageProvenance

        $result.FilePath | Should -Be ([System.IO.Path]::GetFullPath($source))
    }

    It 'throws when the file does not exist' {
        $missing = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/missing.png'

        { Get-ImageProvenance -FilePath $missing -ErrorAction Stop } | Should -Throw
    }
}
