Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "$PSScriptRoot\Samples\Snow.jpeg" | Format-Table

$removeSplat = @{
    FilePath    = "$PSScriptRoot\Samples\Snow.jpeg"
    OutFilePath = "$PSScriptRoot\Output\Snow_NoMeta.jpeg"
}
[ImagePlayground.ImageHelper]::RemoveMetadata($removeSplat.FilePath, $removeSplat.OutFilePath)

Get-ImageExif -FilePath "$PSScriptRoot\Output\Snow_NoMeta.jpeg" | Format-Table
