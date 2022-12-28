Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Samples\LogoEvotecResize.png -Width 100 -Height 100

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Samples\LogoEvotecResizePercent.png -Percentage 200