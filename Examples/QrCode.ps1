Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"
