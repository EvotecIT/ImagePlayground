function Get-ImageQRCode {
    [cmdletBinding()]
    param(
        [string] $FilePath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        $QRCode = [ImagePlayground.QRCode]::Read($FilePath)
        $QRCode
    } else {
        Write-Warning -Message "Get-ImageQRCode - File $FilePath not found. Please check the path."
    }
}