Describe 'Get-ImageQRCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

    }

    It 'reads QR code' {

        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        (Get-ImageQRCode -FilePath $file).Message | Should -Not -BeNullOrEmpty

    }

    It 'does not lock file' {

        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        Get-ImageQRCode -FilePath $file | Out-Null

        {
            [System.IO.File]::Open($file, [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite, [System.IO.FileShare]::None).Dispose()
        } | Should -Not -Throw

    }

}

