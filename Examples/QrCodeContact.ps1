Import-Module .\ImagePlayground.psd1 -Force

New-ImageQRContact -FilePath "$PSScriptRoot\Samples\QRCodeContact.png" -outputType VCard4 -FirstName "Przemek" -Lastname "Klys" -MobilePhone "+48 500 000 000"