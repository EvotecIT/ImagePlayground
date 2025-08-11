#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Rotate-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecRotate90.png -Degrees 90
Rotate-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecRotate180.png -RotateMode Rotate180
