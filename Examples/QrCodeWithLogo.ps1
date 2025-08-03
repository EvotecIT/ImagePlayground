Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

[ImagePlayground.QrCode]::Generate('https://evotec.xyz', "$PSScriptRoot\Samples\QRCodeWithLogo.png", "$PSScriptRoot\Samples\LogoEvotec.png")
