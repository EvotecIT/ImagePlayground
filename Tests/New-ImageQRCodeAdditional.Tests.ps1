describe 'New-ImageQRCode additional cmdlets' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates SMS QR code' {
        $file = Join-Path $TestDir 'sms.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSms -Number '+1234567890' -Message 'Hello' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Match 'SMSTO'
    }

    It 'creates geolocation QR code' {
        $file = Join-Path $TestDir 'geo.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Match 'GEO:'
    }

    It 'creates Bitcoin payment QR code' {
        $file = Join-Path $TestDir 'btc.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Match 'bitcoin:'
    }
}
