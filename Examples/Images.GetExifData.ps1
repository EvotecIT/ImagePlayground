Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$Image = Get-Image -FilePath (Join-Path $PSScriptRoot 'Samples\Snow.jpeg')
try {
    $Image.Width
    $Image.Height
    $Image.Metadata
    $Image.Metadata.ExifValues | Format-Table Tag, DataType, Value
    if ($Image.Metadata.IccProfile) {
        'Embedded ICC profile length: {0} bytes' -f $Image.Metadata.IccProfile.Length
    }
} finally {
    $Image.Dispose()
}
