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
}
