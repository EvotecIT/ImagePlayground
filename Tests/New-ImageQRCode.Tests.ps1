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

}

