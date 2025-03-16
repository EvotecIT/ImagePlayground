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
        [Parameter(Mandatory)]
        [Alias('FullName', 'Path')]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath,
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $OutputPath
    )
    $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
    if (Test-Path -LiteralPath $FilePath -PathType Leaf) {
        [ImagePlayground.ImageHelper]::ConvertTo($FilePath, $OutputPath)
    }
    else {
        Write-Warning -Message "Resize-Image - File $FilePath not found. Please check the path."
    }
}
