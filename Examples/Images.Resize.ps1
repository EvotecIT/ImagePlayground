Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResize.png -Width 100 -Height 100

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizeMaintainAspectRatio.png -Width 300

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizeMaintainAspectRatio.png -Width 300 -DontRespectAspectRatio

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizePercent.png -Percentage 200