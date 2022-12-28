function Get-Image {
    <#
    .SYNOPSIS
    Gets the image from the file path for further processing and image manipulation.

    .DESCRIPTION
    Gets the image from the file path for further processing and image manipulation.

    .PARAMETER FilePath
    File path to the image you want to read and manipulate.

    .EXAMPLE
    $Image = Get-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png
    $Image.BlackWhite()
    $Image.BackgroundColor("Red")
    Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\LogoEvotecChanged.png

    .NOTES
    General notes
    #>
    [CmdletBinding()]
    param(
        [string] $FilePath
    )

    if (-not (Test-Path -LiteralPath $FilePath)) {
        Write-Warning -Message "Get-Image - File $FilePath not found. Please check the path."
        return
    }
    $Image = [ImagePlayground.Image]::Load($FilePath)
    $Image
}