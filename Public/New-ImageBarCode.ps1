function New-ImageBarCode {
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ImagePlayground.BarCode+BarcodeTypes] $Type,
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $Value,
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath
    )
    if ($FilePath) {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
    }

    [ImagePlayground.BarCode]::Generate($Type, $Value, $filePath)
}
