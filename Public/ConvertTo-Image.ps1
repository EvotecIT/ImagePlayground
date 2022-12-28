function ConvertTo-Image {
    [cmdletBinding()]
    param(
        [parameter(Mandatory)][string] $FilePath,
        [parameter(Mandatory)][string] $OutputPath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        [ImagePlayground.ImageHelper]::ConvertTo($FilePath, $OutputPath)
    } else {
        Write-Warning -Message "Resize-Image - File $FilePath not found. Please check the path."
    }
}