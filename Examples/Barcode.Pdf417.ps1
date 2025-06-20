Import-Module .\ImagePlayground.psd1 -Force

$file = Join-Path $PSScriptRoot 'Samples/Pdf417.png'
New-ImageBarCode -Type PDF417 -Value 'Pdf417Example' -FilePath $file
Get-ImageBarCode -FilePath $file
