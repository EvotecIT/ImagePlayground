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
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'sms:|SMSTO'
    }

    It 'creates SMS QR code asynchronously' {
        $file = Join-Path $TestDir 'sms_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSms -Number '+1234567890' -Message 'Hello Async' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'sms:|SMSTO'
    }

    It 'creates email QR code' {
        $file = Join-Path $TestDir 'email.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeEmail -Email 'user@example.com' -Subject 'Hi' -Message 'Body' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'mailto:'
    }

    It 'creates email QR code asynchronously' {
        $file = Join-Path $TestDir 'email_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeEmail -Email 'user@example.com' -Subject 'Hi Async' -Message 'Body Async' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'mailto:'
    }

    It 'creates geolocation QR code' {
        $file = Join-Path $TestDir 'geo.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'GEO:'
    }

    It 'creates geolocation QR code asynchronously' {
        $file = Join-Path $TestDir 'geo_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'GEO:'
    }

    It 'creates Bitcoin payment QR code' {
        $file = Join-Path $TestDir 'btc.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'bitcoin:'
    }

    It 'creates Bitcoin payment QR code asynchronously' {
        $file = Join-Path $TestDir 'btc_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern 'bitcoin:'
    }

    It 'creates Girocode QR code' {
        $file = Join-Path $TestDir 'giro.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Evotec GmbH' -Amount 12.34 -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^BCD'
    }

    It 'creates Girocode QR code asynchronously' {
        $file = Join-Path $TestDir 'giro_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Evotec GmbH' -Amount 12.34 -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^BCD'
    }

    It 'creates Monero QR code' {
        $file = Join-Path $TestDir 'monero.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeMonero -Address '44AFFq5kSiGBoZ' -Amount 1 -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^monero:'
    }

    It 'creates Monero QR code asynchronously' {
        $file = Join-Path $TestDir 'monero_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeMonero -Address '44AFFq5kSiGBoZ' -Amount 1 -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^monero:'
    }

    It 'creates phone number QR code' {
        $file = Join-Path $TestDir 'phone.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodePhoneNumber -Number '+123456' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^tel:'
    }

    It 'creates phone number QR code asynchronously' {
        $file = Join-Path $TestDir 'phone_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodePhoneNumber -Number '+123456' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^tel:'
    }

    It 'creates Skype QR code' {
        $file = Join-Path $TestDir 'skype.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^skype:'
    }

    It 'creates Skype QR code asynchronously' {
        $file = Join-Path $TestDir 'skype_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^skype:'
    }

    It 'throws on invalid pixel size for Sms' {
        { New-ImageQRCodeSms -Number '+1234567890' -Message 'Hello' -FilePath (Join-Path $TestDir 'sms_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Email' {
        { New-ImageQRCodeEmail -Email 'user@example.com' -FilePath (Join-Path $TestDir 'email_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for GeoLocation' {
        { New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath (Join-Path $TestDir 'geo_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Bitcoin' {
        { New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath (Join-Path $TestDir 'btc_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Girocode' {
        { New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Evotec GmbH' -Amount 12.34 -FilePath (Join-Path $TestDir 'giro_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Monero' {
        { New-ImageQRCodeMonero -Address '44AFFq5kSiGBoZ' -Amount 1 -FilePath (Join-Path $TestDir 'monero_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for PhoneNumber' {
        { New-ImageQRCodePhoneNumber -Number '+123456' -FilePath (Join-Path $TestDir 'phone_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for SkypeCall' {
        { New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath (Join-Path $TestDir 'skype_invalid.png') -PixelSize 0 } | Should -Throw
    }
}
