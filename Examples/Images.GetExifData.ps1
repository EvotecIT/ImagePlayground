# Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$Image = Get-Image -FilePath "$PSScriptRoot\Samples\Snow.jpeg"
$Image.Width
$Image.Height
$Image.Metadata
$Image.Metadata.ExifProfile | Format-List
$Image.Metadata.ExifProfile.Values | Format-Table
$Image.Metadata.IccProfile.Header | Format-Table
$Image.Metadata.IccProfile.Entries | Format-Table
