function Merge-Image {
    [CmdletBinding()]
    param(
        [string] $FilePath,
        [string] $FilePathToMerge,
        [string] $FilePathOutput,
        [switch] $ResizeToFit,
        [ImagePlayground.ImagePlacement] $Placement = [ImagePlayground.ImagePlacement]::Bottom
    )
    if (-not (Test-Path -LiteralPath $FilePath)) {
        Write-Warning -Message "Merge-Image - File $FilePath not found. Please check the path."
        return
    }
    if (-not (Test-Path -LiteralPath $FilePathToMerge)) {
        Write-Warning -Message "Merge-Image - File $FilePathToMerge not found. Please check the path."
        return
    }
    [ImagePlayground.ImageHelper]::Combine($FilePath, $FilePathToMerge, $FilePathOutput, $ResizeToFit.IsPresent, $Placement)
}