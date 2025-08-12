Describe 'New-ImageIcon' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates icon with selected sizes' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'icon.ico'
        if (Test-Path $dest) { Remove-Item $dest }
        New-ImageIcon -FilePath $src -OutputPath $dest -Size @(16,32)
        Test-Path $dest | Should -BeTrue
    }

    It 'creates icon in non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'icon.ico'
        New-ImageIcon -FilePath $src -OutputPath $dest -Size @(16,32)
        Test-Path $dest | Should -BeTrue
    }
}
