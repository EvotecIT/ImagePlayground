Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

#Add-Type -Path "C:\Support\GitHub\ImagePlayground\Lib\Default\System.Numerics.Vectors.dll" -PassThru

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"
New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCodeTransparent.png"
