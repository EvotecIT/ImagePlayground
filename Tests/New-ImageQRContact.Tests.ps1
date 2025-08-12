Describe 'New-ImageQRContact' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates contact QR code' {
        $file = Join-Path $TestDir 'contact.png'
        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRContact -FilePath $file -Firstname 'John' -Lastname 'Doe'

        Test-Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Match 'BEGIN:VCARD'
    }

    It 'creates contact QR in non-existent directory' {
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $file = Join-Path $dir 'contact.png'
        New-ImageQRContact -FilePath $file -Firstname 'John' -Lastname 'Doe'
        Test-Path $file | Should -BeTrue
    }

    It 'throws on invalid pixel size' {
        { New-ImageQRContact -FilePath (Join-Path $TestDir 'contact_invalid.png') -Firstname 'John' -Lastname 'Doe' -PixelSize 0 } | Should -Throw
    }
}
