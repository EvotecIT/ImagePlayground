Describe 'New-ImageQRCode specialized cmdlets' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

        function New-TestSwissQrPayload {
            $iban = [CodeGlyphX.Payloads.SwissQrCodePayload+Iban]::new(
                'CH4431999123000889012',
                [CodeGlyphX.Payloads.SwissQrCodePayload+Iban+IbanType]::Iban)
            $creditor = [CodeGlyphX.Payloads.SwissQrCodePayload+Contact]::CreateStructured(
                'Evotec GmbH',
                'Main Street',
                '1',
                '8000',
                'Zurich',
                'CH')
            $reference = [CodeGlyphX.Payloads.SwissQrCodePayload+Reference]::new(
                [CodeGlyphX.Payloads.SwissQrCodePayload+Reference+ReferenceType]::NON)

            [CodeGlyphX.Payloads.SwissQrCodePayload]::new(
                $iban,
                [CodeGlyphX.Payloads.QrSwissCurrency]::CHF,
                $creditor,
                $reference)
        }

        function New-TestSlovenianUpnPayload {
            [CodeGlyphX.Payloads.SlovenianUpnQrPayload]::new(
                'John Doe',
                'Main Street 1',
                'Ljubljana',
                'Evotec d.o.o.',
                'Business Street 2',
                'Maribor',
                'SI56192001234567890',
                'Invoice 2026-041',
                199.99)
        }
    }

    It 'creates OTP QR code' {
        $file = Join-Path $TestDir 'otp.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeOtp -Type Totp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'john.doe@evotec.pl' -Issuer 'Evotec' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^otpauth://'
    }

    It 'creates OTP QR code asynchronously' {
        $file = Join-Path $TestDir 'otp_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeOtp -Type Totp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'john.doe@evotec.pl' -Issuer 'Evotec' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^otpauth://'
    }

    It 'creates Shadowsocks QR code' {
        $file = Join-Path $TestDir 'shadowsocks.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'secret' -Method Aes256Gcm -Tag 'Warsaw' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^ss://'
    }

    It 'creates Shadowsocks QR code asynchronously' {
        $file = Join-Path $TestDir 'shadowsocks_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'secret' -Method Aes256Gcm -Tag 'Warsaw' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^ss://'
    }

    It 'creates BezahlCode QR code' {
        $file = Join-Path $TestDir 'bezahl.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority Contact -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Reason 'Invoice 2026-041' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://contact'
    }

    It 'creates BezahlCode QR code asynchronously' {
        $file = Join-Path $TestDir 'bezahl_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority Contact -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Reason 'Invoice 2026-041' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://contact'
    }

    It 'creates BezahlCode contact v2 SEPA QR code' {
        $file = Join-Path $TestDir 'bezahl_contact_v2_sepa.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority ContactV2 -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Reason 'Invoice 2026-041' -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://contact_v2'
    }

    It 'creates BezahlCode single payment QR code' {
        $file = Join-Path $TestDir 'bezahl_single.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Amount 12.34 -Reason 'Invoice 2026-041' -ExecutionDate ([datetime]'2026-04-10') -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://singlepayment'
    }

    It 'creates BezahlCode periodic SEPA QR code asynchronously' {
        $file = Join-Path $TestDir 'bezahl_periodic_sepa_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority PeriodicSinglePaymentSepa -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 29.99 -Reason 'Support Retainer' -PeriodicUnit Monthly -PeriodicUnitRotation 1 -PeriodicFirstExecutionDate ([datetime]'2026-05-01') -PeriodicLastExecutionDate ([datetime]'2026-12-01') -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://periodicsinglepaymentsepa'
    }

    It 'creates BezahlCode direct debit SEPA QR code' {
        $file = Join-Path $TestDir 'bezahl_direct_debit_sepa.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeBezahlCode -Authority SingleDirectDebitSepa -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 45.67 -Reason 'Invoice 2026-041' -CreditorId 'DE98ZZZ09999999999' -MandateId 'MANDATE-2026-041' -DateOfSignature ([datetime]'2026-03-01') -ExecutionDate ([datetime]'2026-04-12') -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^bank://singledirectdebitsepa'
    }

    It 'creates Swiss QR code' {
        $file = Join-Path $TestDir 'swiss.png'
        if (Test-Path $file) { Remove-Item $file }
        $payload = New-TestSwissQrPayload
        New-ImageQRCodeSwiss -Payload $payload -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Swiss QR code asynchronously' {
        $file = Join-Path $TestDir 'swiss_async.png'
        if (Test-Path $file) { Remove-Item $file }
        $payload = New-TestSwissQrPayload
        New-ImageQRCodeSwiss -Payload $payload -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Slovenian UPN QR code' {
        $file = Join-Path $TestDir 'upn.png'
        if (Test-Path $file) { Remove-Item $file }
        $payload = New-TestSlovenianUpnPayload
        New-ImageQRCodeSlovenianUpnQr -Payload $payload -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^UPNQR'
    }

    It 'creates Slovenian UPN QR code asynchronously' {
        $file = Join-Path $TestDir 'upn_async.png'
        if (Test-Path $file) { Remove-Item $file }
        $payload = New-TestSlovenianUpnPayload
        New-ImageQRCodeSlovenianUpnQr -Payload $payload -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^UPNQR'
    }

    It 'throws on invalid pixel size for OTP' {
        { New-ImageQRCodeOtp -Type Totp -SecretBase32 'JBSWY3DPEHPK3PXP' -FilePath (Join-Path $TestDir 'otp_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for ShadowSocks' {
        { New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'secret' -Method Aes256Gcm -FilePath (Join-Path $TestDir 'shadowsocks_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws when amount is missing for BezahlCode payment authority' {
        { New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Account '1234567890' -Bnc '10020030' -Reason 'Invoice 2026-041' -FilePath (Join-Path $TestDir 'bezahl_missing_amount.png') } | Should -Throw
    }

    It 'throws when creditor details are missing for BezahlCode direct debit authority' {
        {
            New-ImageQRCodeBezahlCode -Authority SingleDirectDebitSepa -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 45.67 -Reason 'Invoice 2026-041' -MandateId 'MANDATE-2026-041' -DateOfSignature ([datetime]'2026-03-01') -FilePath (Join-Path $TestDir 'bezahl_missing_creditor.png')
        } | Should -Throw '*CreditorId*'
    }

    It 'throws when authority does not match the SEPA payment parameter set' {
        $errorRecord = $null

        try {
            New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Evotec GmbH' -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Amount 12.34 -Reason 'Invoice 2026-041' -FilePath (Join-Path $TestDir 'bezahl_invalid_authority_set.png') -ErrorAction Stop
        } catch {
            $errorRecord = $_
        }

        $errorRecord | Should -Not -BeNullOrEmpty
        $errorRecord.Exception.Message | Should -Match 'Authority SinglePayment is not valid for parameter set SepaPayment\.'
    }

    It 'throws on invalid pixel size for Swiss' {
        $payload = New-TestSwissQrPayload
        { New-ImageQRCodeSwiss -Payload $payload -FilePath (Join-Path $TestDir 'swiss_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Slovenian UPN' {
        $payload = New-TestSlovenianUpnPayload
        { New-ImageQRCodeSlovenianUpnQr -Payload $payload -FilePath (Join-Path $TestDir 'upn_invalid.png') -PixelSize 0 } | Should -Throw
    }
}
