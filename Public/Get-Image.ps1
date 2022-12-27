function Get-Image {
    [CmdletBinding()]
    param(
        [string] $FilePath
    )

    if (-not (Test-Path -LiteralPath $FilePath)) {
        Write-Warning -Message "Get-Image - File $FilePath not found. Please check the path."
        return
    }
    $Image = [ImagePlayground.Image]::Load($FilePath)
    $Image
}