function New-ImageQRCode {
    [Alias('New-QRCode')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][string] $FilePath
    )
    if (-not $FilePath) {
        $FilePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($([System.IO.Path]::GetRandomFileName()).Split('.')[0]).png")
        Write-Warning -Message "New-ImageQRCode - No file path specified, saving to $FilePath"
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