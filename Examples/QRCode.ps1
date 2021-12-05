Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

#New-QRCode -FilePath "$PSScriptRoot\Samples\QRcode.png" -Content 'https://evotec.xyz' -FileFormat png
#New-QRCode -FilePath "$PSScriptRoot\Samples\QRcode.webp" -Content 'https://evotec.xyz' -FileFormat webp

$Settings = [NetBarcode.BarcodeSettings]::new()
$Settings.Text = '10500400412728169'
$Test = [NetBarcode.Barcode]::new()
$null = $Test.Configure($Settings)
$Test.SaveImageFile('10500400412728169', "$PSScriptRoot\Samples\BarCode.png",[System.Drawing.Imaging.ImageFormat]::Png)