function ConvertTo-Image {
    <#
    .SYNOPSIS
    Converts the image to the specified format.

    .DESCRIPTION
    Converts the image to the specified format. The output path must include the file extension.

    .PARAMETER FilePath
    File path to the image you want to convert.

    .PARAMETER OutputPath
    File path to the output image that will be created.

    .EXAMPLE
    ConvertTo-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotec.jpg

    .NOTES
    General notes
    #>
    [cmdletBinding()]
    param(
        [parameter(Mandatory)][string] $FilePath,
        [parameter(Mandatory)][string] $OutputPath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        [ImagePlayground.ImageHelper]::ConvertTo($FilePath, $OutputPath)
    } else {
        Write-Warning -Message "Resize-Image - File $FilePath not found. Please check the path."
    }
}