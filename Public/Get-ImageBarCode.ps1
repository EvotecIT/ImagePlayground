function Get-ImageBarCode {
    [cmdletBinding()]
    param(
        [string] $FilePath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        $BarCode = [ImagePlayground.BarCode]::Read($FilePath)
        $BarCode
    } else {
        Write-Warning -Message "Get-ImageBarCode - File $FilePath not found. Please check the path."
    }
}