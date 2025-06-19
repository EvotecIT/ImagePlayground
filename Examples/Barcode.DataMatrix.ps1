Import-Module .\ImagePlayground.psd1 -Force

$file = Join-Path $PSScriptRoot 'Samples/DataMatrix.png'
New-ImageBarCode -Type DataMatrix -Value 'DataMatrixExample' -FilePath $file
Get-ImageBarCode -FilePath $file
