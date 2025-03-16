# Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRContact -FilePath "$PSScriptRoot\Output\QRCodeContact.png" -outputType VCard4 -Firstname "Przemek" -Lastname "Klys" -MobilePhone "+48 500 000 000"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Output\QRCodeContact.png"
$Message | Format-List *

New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'EvotecPassword' -FilePath "$PSScriptRoot\Output\QRCodeWiFi.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Output\QRCodeWiFi.png"
$Message | Format-List *

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Output\QRCodeTest.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Output\QRCodeTest.png"
$Message | Format-List *
