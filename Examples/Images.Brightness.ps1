
$File = ".\Samples\PrzemyslawKlysAndKulkozaurr.jpg"

$Image = Get-Image -FilePath $File
$Image.Brightness(0.5)
$Image.Save($Image.FilePath.Replace('.jpg', '_Brightness1.jpg'))
$Image.Brightness(0.1)
$Image.Save($Image.FilePath.Replace('.jpg', '_Brightness2.jpg'))
