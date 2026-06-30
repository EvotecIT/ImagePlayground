Describe 'New-ImageQRCode specialized cmdlets' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
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
        New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Swiss QR code asynchronously' {
        $file = Join-Path $TestDir 'swiss_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -Amount 199.99 -UnstructuredMessage 'Invoice 2026-041' -FilePath $file -Async
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Swiss QR code with combined creditor address' {
        $file = Join-Path $TestDir 'swiss_combined.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorAddressType CombinedAddress -CreditorName 'Evotec GmbH' -CreditorAddressLine1 'Main Street 1' -CreditorAddressLine2 '8000 Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Swiss QR code with empty combined creditor address line 1' {
        $file = Join-Path $TestDir 'swiss_combined_empty_line1.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorAddressType CombinedAddress -CreditorName 'Evotec GmbH' -CreditorAddressLine2 '8000 Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'creates Swiss QR code with structured creditor postal address only' {
        $file = Join-Path $TestDir 'swiss_postal_address.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^SPC'
    }

    It 'exposes Swiss QR parameters through CodeGlyphX public enums' {
        $parameters = (Get-Command New-ImageQRCodeSwiss).Parameters

        $parameters['IbanType'].ParameterType.FullName | Should -Be 'CodeGlyphX.SwissQrIbanType'
        $parameters['Currency'].ParameterType.FullName | Should -Be 'CodeGlyphX.SwissQrCurrency'
        $parameters['CreditorAddressType'].ParameterType.FullName | Should -Be 'CodeGlyphX.SwissQrAddressType'
        $parameters['ReferenceType'].ParameterType.FullName | Should -Be 'CodeGlyphX.SwissQrReferenceType'
        $parameters['DebtorAddressType'].ParameterType.FullName | Should -Be 'CodeGlyphX.SwissQrAddressType'
        $parameters.Keys | Should -Not -Contain 'ReferenceTextType'
    }

    It 'exposes Swiss QR payload value-object accelerators for static helpers' {
        $expectedAccelerators = @(
            'CodeGlyphX.Payloads.SwissQrCodePayload'
            'CodeGlyphX.Payloads.SwissQrCodePayload+AdditionalInformation'
            'CodeGlyphX.Payloads.SwissQrCodePayload+Contact'
            'CodeGlyphX.Payloads.SwissQrCodePayload+Iban'
            'CodeGlyphX.Payloads.SwissQrCodePayload+Reference'
        )
        $legacyAccelerators = @(
            'CodeGlyphX.Payloads.SwissQrCodePayload+Iban+IbanType'
            'CodeGlyphX.Payloads.SwissQrCodePayload+Reference+ReferenceType'
            'CodeGlyphX.Payloads.SwissQrCodePayload+Reference+ReferenceTextType'
        )
        $moduleScriptPath = Join-Path -Path (Get-Module -Name ImagePlayground).ModuleBase -ChildPath 'ImagePlayground.psm1'
        $moduleScript = Get-Content -Path $moduleScriptPath -Raw
        if ($moduleScript -notmatch 'RegisterPowerForgeAssemblyTypeAccelerators') {
            $buildScript = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Build\Build-Module.ps1') -Raw
            foreach ($accelerator in $expectedAccelerators) {
                $buildScript | Should -Match ([regex]::Escape($accelerator))
            }

            foreach ($accelerator in $legacyAccelerators) {
                $buildScript | Should -Not -Match ([regex]::Escape($accelerator))
            }

            return
        }

        $typeAccelerators = [psobject].Assembly.GetType('System.Management.Automation.TypeAccelerators')::Get

        foreach ($accelerator in $expectedAccelerators) {
            $typeAccelerators.ContainsKey($accelerator) | Should -BeTrue
        }

        foreach ($accelerator in $legacyAccelerators) {
            $typeAccelerators.ContainsKey($accelerator) | Should -BeFalse
        }
    }

    It 'requires Swiss QR reference text for QRR and SCOR' {
        {
            New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType SCOR -FilePath (Join-Path $TestDir 'swiss_missing_scor_reference.png')
        } | Should -Throw '*Reference is required*'
    }

    It 'creates Slovenian UPN QR code' {
        $file = Join-Path $TestDir 'upn.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -FilePath $file
        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedPattern '^UPNQR'
    }

    It 'creates Slovenian UPN QR code asynchronously' {
        $file = Join-Path $TestDir 'upn_async.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -Deadline ([datetime]'2026-04-10') -RecipientSiModel 'SI00' -RecipientSiReference '2026041' -FilePath $file -Async
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
        { New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath (Join-Path $TestDir 'swiss_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'throws on invalid pixel size for Slovenian UPN' {
        { New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -FilePath (Join-Path $TestDir 'upn_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'does not expose legacy Swiss or Slovenian payload parameters' {
        (Get-Command New-ImageQRCodeSwiss).Parameters.Keys | Should -Not -Contain 'Payload'
        (Get-Command New-ImageQRCodeSlovenianUpnQr).Parameters.Keys | Should -Not -Contain 'Payload'
    }

    It 'does not expose unsupported Swiss ultimate creditor parameters' {
        (Get-Command New-ImageQRCodeSwiss).Parameters.Keys -like 'UltimateCreditor*' | Should -BeNullOrEmpty
    }
}
