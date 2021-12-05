Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-QRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode1.png"
New-QRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode2.png" -Transparent