﻿Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

#New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode1.png"
#New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode2.png" -Transparent


New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'Evotec' -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"