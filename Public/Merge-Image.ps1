function Merge-Image {
    <#
    .SYNOPSIS
    Merges two images into a single image

    .DESCRIPTION
    Merges two images into a single image

    .PARAMETER FilePath
    File path to image to be processed for merging of images. This image will be the base image.

    .PARAMETER FilePathToMerge
    File path to image to be merged into the base image.

    .PARAMETER FilePathOutput
    File path to output image.

    .PARAMETER ResizeToFit
    Resize the image to fit the base image. If not specified, the image will be added as is.

    .PARAMETER Placement
    Placement of the image to be merged. Default is bottom. Possible values: Top, Bottom, Left, Right

    .EXAMPLE
    Merge-Image -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -FilePathToMerge "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -FilePathOutput "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -ResizeToFit -Placement Bottom

    .NOTES
    General notes
    #>
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