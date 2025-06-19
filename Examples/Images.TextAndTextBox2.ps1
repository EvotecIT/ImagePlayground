Import-Module $PSScriptRoot/..\ImagePlayground.psd1 -Force

$Sample = Join-Path $PSScriptRoot 'Samples/PrzemyslawKlysAndKulkozaurr.jpg'
$Image = Get-Image -FilePath $Sample

$Image.AddText(10, 10, 'Top-left text', [SixLabors.ImageSharp.Color]::Green, 24)
$Image.AddTextBox(10, 40, 'Add-TextBox with narrow width wraps quickly for comparison.', 150, [SixLabors.ImageSharp.Color]::Orange, 24)

$Output = Join-Path $PSScriptRoot 'Output/TextAndTextBox2.jpg'
Save-Image -Image $Image -FilePath $Output
