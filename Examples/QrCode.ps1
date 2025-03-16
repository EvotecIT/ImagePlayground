# Import-Module .\ImagePlayground.psd1 -Force

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Output\QRCode.png" -Verbose
