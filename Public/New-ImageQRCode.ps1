function New-QRCode {
    [Alias('New-ImageQRCode')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][string] $FilePath,
        [switch] $Transparent
    )

    [ImagePlayground.QrCode]::Generate($Content, $FilePath, $Transparent.IsPresent)
}