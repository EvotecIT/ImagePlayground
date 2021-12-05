Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageBarCode -Content '10500400412728169' -FilePath "$PSScriptRoot\Samples\BarCode1.png" -Height 50
New-ImageBarCode -Content '10500400412728169' -FilePath "$PSScriptRoot\Samples\BarCode2.png" -Rotate Rotate180FlipXY -HideLabel