function New-QRCode {
    [Alias('New-ImageQRCode')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][string] $FilePath
    )

    [ImagePlayground.QrCode]::Generate($Content, $FilePath, $false)
}