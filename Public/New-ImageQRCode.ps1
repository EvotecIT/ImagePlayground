function New-ImageQRCode {
    <#
    .SYNOPSIS
    Creates QR code

    .DESCRIPTION
    Creates QR code

    .PARAMETER Content
    Content to be encoded in QR code

    .PARAMETER FilePath
    File path to where the image with QR code should be saved.

    .EXAMPLE
    New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png" -Verbose

    .NOTES
    General notes
    #>
    [Alias('New-QRCode')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $Content,
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath
    )
    if ($FilePath) {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
    }
    try {
        [ImagePlayground.QrCode]::Generate($Content, $FilePath, $false)
        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            throw
        } else {
            Write-Warning -Message "New-ImageQRCodeWiFi - Error creating image $($_.Exception.Message)"
        }
    }
}
