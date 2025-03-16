# Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'Evotec' -FilePath "$PSScriptRoot\Output\QRCodeWiFi.png"
