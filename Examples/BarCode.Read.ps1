Import-Module .\ImagePlayground.psd1 -Force

Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN13.png
Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN7.png