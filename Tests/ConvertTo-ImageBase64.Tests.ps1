describe 'ConvertTo-ImageBase64' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }
    It 'returns base64 string' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $b64 = ConvertTo-ImageBase64 -FilePath $src
        $b64 | Should -Not -BeNullOrEmpty
    }
}
