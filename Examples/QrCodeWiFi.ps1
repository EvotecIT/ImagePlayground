Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'Evotec;123!' -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"
