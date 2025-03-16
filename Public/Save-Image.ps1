function Save-Image {
    <#
    .SYNOPSIS
    Saves image that was open for processing

    .DESCRIPTION
    Saves image that was open for processing

    .PARAMETER Image
    Image object to be saved

    .PARAMETER FilePath
    File path to where the image should be saved. If not specified, the original image will be overwritten.

    .PARAMETER Open
    Opens the image in default image viewer after saving

    .EXAMPLE
    $Image = Get-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png
    $Image.BlackWhite()
    $Image.BackgroundColor("Red")
    Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\LogoEvotecChanged.png

    .NOTES
    General notes
    #>
    [cmdletBinding()]
    param(
        [parameter(Mandatory)]
        [ImagePlayground.Image] $Image,
        [ValidateNotNullOrEmpty()]
        [string] $FilePath,
        [switch] $Open
    )
    if ($FilePath) {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
        $Image.Save($FilePath, $Open.IsPresent)
    } else {
        $Image.Save($Open.IsPresent)
    }
}
