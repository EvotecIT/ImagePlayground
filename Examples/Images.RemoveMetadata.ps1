Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "$PSScriptRoot\Samples\Snow.jpeg" | Format-Table

$removeSplat = @{
    FilePath   = "$PSScriptRoot\Samples\Snow.jpeg"
    OutputPath = "$PSScriptRoot\Output\Snow_NoMeta.jpeg"
}
Remove-ImageMetadata @removeSplat

Get-ImageExif -FilePath "$PSScriptRoot\Output\Snow_NoMeta.jpeg" | Format-Table
