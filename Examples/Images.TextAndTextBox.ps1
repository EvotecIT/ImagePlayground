Import-Module $PSScriptRoot/..\ImagePlayground.psd1 -Force

$Sample = Join-Path $PSScriptRoot 'Samples/PrzemyslawKlysAndKulkozaurr.jpg'
$Image = Get-Image -FilePath $Sample

$Image.AddText(50, 50, 'Add-Text example', [OfficeIMO.Drawing.OfficeColor]::Red, 32)

$Image.AddTextBox(50, 100, 'Add-TextBox wraps this very long line of text inside a specified width to show the difference.', 400, [OfficeIMO.Drawing.OfficeColor]::Blue, 32)

$Output = Join-Path $PSScriptRoot 'Output/TextAndTextBox.jpg'
Save-Image -Image $Image -FilePath $Output

