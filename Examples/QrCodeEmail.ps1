Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCodeEmail -Email 'user@example.com' -Subject 'Hello' -Message 'Body' -FilePath "$PSScriptRoot\Samples\QRCodeEmail.png"

