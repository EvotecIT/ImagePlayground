Import-Module .\ImagePlayground.psd1 -Force

New-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN13.png -Type EAN -Value "5901234123457"
New-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN7.png -Type EAN -Value "5901234"