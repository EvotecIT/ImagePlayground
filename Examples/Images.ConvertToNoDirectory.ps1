Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Push-Location $PSScriptRoot
ConvertTo-Image -FilePath .\Samples\LogoEvotec.png -OutputPath LogoEvotec.jpg
Pop-Location
