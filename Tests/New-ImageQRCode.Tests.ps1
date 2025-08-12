Describe 'New-ImageQRCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'creates QR code' {

        $file = Join-Path $TestDir 'qr.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path $file | Should -BeTrue

        (Get-ImageQRCode -FilePath $file).Message | Should -Be 'https://evotec.xyz'

    }

    It 'creates QR code in current directory' {

        $file = 'qr_current.png'

        if (Test-Path -Path $file) { Remove-Item -Path $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path -Path $file | Should -BeTrue

        (Get-ImageQRCode -FilePath $file).Message | Should -Be 'https://evotec.xyz'

        Remove-Item -Path $file

    }

    It 'creates QR code with custom colors' {

        $file = Join-Path $TestDir 'qr_custom.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -ForegroundColor Red -BackgroundColor Yellow -PixelSize 10

        Test-Path $file | Should -BeTrue

        (Get-ImageQRCode -FilePath $file).Message | Should -Be 'https://evotec.xyz'

    }

    It 'creates QR code icon' {

        $file = Join-Path $TestDir 'qr.ico'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path $file | Should -BeTrue

    }

    It 'throws on invalid pixel size' {
        { New-ImageQRCode -Content 'https://evotec.xyz' -FilePath (Join-Path $TestDir 'qr_invalid.png') -PixelSize 0 } | Should -Throw
    }

}

