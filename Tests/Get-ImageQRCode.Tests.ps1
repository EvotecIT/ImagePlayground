Describe 'Get-ImageQRCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

    }

    It 'reads QR code' {

        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        (Get-ImageQRCode -FilePath $file).Message | Should -NotBeNullOrEmpty

    }

}

