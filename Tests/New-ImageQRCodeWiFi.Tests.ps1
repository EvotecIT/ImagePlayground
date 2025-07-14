Describe 'New-ImageQRCodeWiFi password quoting' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'does not quote numeric passwords' {

        $file = Join-Path $TestDrive 'wifi_numeric.png'

        New-ImageQRCodeWiFi -SSID 'Test' -Password '12345678' -FilePath $file

        $result = Get-ImageQRCode -FilePath $file

        $result.Message | Should -Be 'WIFI:T:WPA;S:Test;P:12345678;;'

    }

    It 'handles alphanumeric passwords' {

        $file = Join-Path $TestDrive 'wifi_alpha.png'

        New-ImageQRCodeWiFi -SSID 'Test' -Password 'pass123' -FilePath $file

        $result = Get-ImageQRCode -FilePath $file

        $result.Message | Should -Be 'WIFI:T:WPA;S:Test;P:pass123;;'

    }

    It 'handles passwords with symbols' {

        $file = Join-Path $TestDrive 'wifi_symbols.png'
        New-ImageQRCodeWiFi -SSID 'Test' -Password 'pass;123!' -FilePath $file
        $result = Get-ImageQRCode -FilePath $file
        $result.Message | Should -Be 'WIFI:T:WPA;S:Test;P:pass%3B123!;;'
    }
}

