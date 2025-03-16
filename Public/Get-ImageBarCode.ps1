function Get-ImageBarCode {
    <#
    .SYNOPSIS
    Gets bar code from image

    .DESCRIPTION
    Gets bar code from image

    .PARAMETER FilePath
    File path to image to be processed for bar code reading

    .EXAMPLE
    Get-ImageBarCode -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"

    .NOTES
    General notes
    #>
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [Alias('FullName', 'Path')]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath
    )
    try {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
        if (-Not (Test-Path -LiteralPath $FilePath -PathType Leaf)) {
            Write-Warning -Message "Get-ImageBarCode - File $FilePath not found. Please check the path."
        }
        [ImagePlayground.BarCode]::Read($FilePath)
    }
    catch {
        Write-Error $_
    }
}
