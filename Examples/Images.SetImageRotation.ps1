#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Set-ImageRotation -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecRotate90.png -Degrees 90
Set-ImageRotation -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecRotate180.png -RotateMode Rotate180
