describe 'ConvertFrom-ImageBase64' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }
    It 'creates image from base64' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'fromb64.png'
        if (Test-Path $dest) { Remove-Item $dest }
        $b64 = ConvertTo-ImageBase64 -FilePath $src
        ConvertFrom-ImageBase64 -Base64 $b64 -OutputPath $dest
        Test-Path $dest | Should -BeTrue
    }

    It 'creates image in non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $b64 = ConvertTo-ImageBase64 -FilePath $src
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'fromb64.png'
        ConvertFrom-ImageBase64 -Base64 $b64 -OutputPath $dest
        Test-Path $dest | Should -BeTrue
    }
}
