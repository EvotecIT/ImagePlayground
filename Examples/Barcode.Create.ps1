Import-Module .\ImagePlayground.psd1 -Force

New-ImageBarCode -FilePath C:\Temp\BarcodeEAN13.png -Type EAN -Value "5901234123457"
New-ImageBarCode -FilePath C:\Temp\BarcodeEAN7.png -Type EAN -Value "5901234"

Get-ImageBarCode -FilePath C:\Temp\BarcodeEAN13.png
Get-ImageBarCode -FilePath C:\Temp\BarcodeEAN7.png
