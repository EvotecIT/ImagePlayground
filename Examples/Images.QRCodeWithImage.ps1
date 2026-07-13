Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Output\QRCodeWithImage.png" -LogoPath "$PSScriptRoot\Samples\LogoEvotec.png" -Show
